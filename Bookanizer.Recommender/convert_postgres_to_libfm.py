'''
Tool to load PostgreSQL data and convert it into LibFM data.
'''

import argparse
from enum import Enum
import psycopg2
from psycopg2 import sql
import random

class ReadLocationEnum(Enum):
    HOME = 1
    TRANSIT = 2
    PUBLIC = 3

def get_distinct_rows(conn: psycopg2.connection, columns: list, table: str):
    with conn.cursor() as cursor:
        cursor.execute(
            sql.SQL("SELECT DISTINCT {columns} FROM {table}").format(
                columns=sql.SQL(", ").join(sql.Identifier(column) for column in columns),
                table=sql.Identifier(table)
            )
        )
        for row in cursor:
            yield row

def main(host: str, port: int, dbname: str, user: str, password: str, train_ratio: float, validation_ratio: float):
    # Validate ratios
    if train_ratio <= 0:
        raise ValueError("Train ratio must be above 0")
    if train_ratio >= 1:
        raise ValueError("Train ratio must be below 1")
    if validation_ratio <= 0:
        raise ValueError("Validation ratio must be above 0")
    if validation_ratio >= 1:
        raise ValueError("Validation ratio must be below 1")
    if train_ratio + validation_ratio >= 1:
        raise ValueError("Train ratio + validation ratio must be below 1")
    
    # Connect to DB
    with psycopg2.connect(host=host, port=port, dbname=dbname, user=user, password=password) as conn:
        # Build feature index map
        feature_map = {}
        index = 0

        # Categorical features
        for (user_id,) in get_distinct_rows(conn, ["user_id"], "interactions"):
            feature_map[f"user_id:{user_id}"] = index
            index += 1

        for read_location in ReadLocationEnum:
            feature_map[f"read_location:{read_location.name}"] = index
            index += 1

        for (book_id,) in get_distinct_rows(conn, ["book_id"], "books"):
            feature_map[f"book_id:{book_id}"] = index
            index += 1

        for (country_code,) in get_distinct_rows(conn, ["country_code"], "books"):
            feature_map[f"country_code:{country_code}"] = index
            index += 1

        for (language_code,) in get_distinct_rows(conn, ["language_code"], "books"):
            feature_map[f"language_code:{language_code}"] = index
            index += 1

        for (author_id,) in get_distinct_rows(conn, ["author_id"], "books"):
            feature_map[f"author_id:{author_id}"] = index
            index += 1

        for (genre_id,) in get_distinct_rows(conn, ["genre_id"], "book_genres"):
            feature_map[f"genre_id:{genre_id}"] = index
            index += 1

        # Numerical features
        feature_map["average_rating"] = index
        index += 1

        feature_map["ratings_count"] = index
        index += 1

        feature_map["num_pages"] = index
        index += 1

        feature_map["publication_year"] = index
        index += 1

        # Stream PostgreSQL data and write
        with open("bookanizer.data.train.libfm", "w") as train_file, open("bookanizer.data.validation.libfm", "w") as validation_file, open("bookanizer.data.test.libfm", "w") as test_file:
            cursor = conn.cursor("recommendations_cursor")  # Server-side named cursor as this query is huge
            cursor.execute("""
                SELECT
                    i.user_id,
                    i.rating,
                    i.read_location,
                    b.book_id,
                    b.country_code,
                    b.language_code,
                    b.average_rating,
                    b.ratings_count,
                    b.author_id,
                    b.num_pages,
                    EXTRACT(YEAR FROM b.publication_date) AS publication_year,
                    array_agg(bg.genre_id) AS genre_ids
                FROM interactions i
                JOIN books b ON i.book_id = b.book_id
                LEFT JOIN book_genres bg ON i.book_id = bg.book_id
                GROUP BY
                    i.user_id,
                    i.book_id,
                    i.rating,
                    i.read_location,
                    b.country_code,
                    b.language_code,
                    b.average_rating,
                    b.ratings_count,
                    b.author_id,
                    b.num_pages,
                    EXTRACT(YEAR FROM b.publication_date)
            """)
            
            # Write to sets
            train_threshold = train_ratio
            validation_threshold = train_ratio + validation_ratio
            for (user_id, rating, read_location, book_id, country_code, language_code, average_rating, ratings_count, author_id, num_pages, publication_year, genre_ids) in cursor:
                #########################################
                # Tuple Content                         #
                #########################################
                # INDEX # CONTENT          # TYPE       #
                #########################################
                # 0     # user_id          # str        #
                # 1     # rating           # float      #
                # 2     # read_location    # int?       #
                # 3     # book_id          # int        #
                # 4     # country_code     # str?       #
                # 5     # language_code    # str?       #
                # 6     # average_rating   # float?     #
                # 7     # ratings_count    # int?       #
                # 8     # author_id        # int        #
                # 9     # num_pages        # int?       #
                # 10    # publication_year # float?     #
                # 11    # genre_ids        # list[int?] #
                #########################################
                
                # Skip line if rating (our label) is null
                if rating is None:
                    continue

                # Format data
                label = rating
                features = []

                # Categorical features
                user_id_feature = feature_map[f"user_id:{user_id}"]
                features.append(f"{user_id_feature}:1")

                if read_location is not None:
                    read_location_name = ReadLocationEnum(read_location).name
                    read_location_feature = feature_map[f"read_location:{read_location_name}"]
                    features.append(f"{read_location_feature}:1")

                book_id_feature = feature_map[f"book_id:{book_id}"]
                features.append(f"{book_id_feature}:1")

                if country_code is not None:
                    country_code_feature = feature_map[f"country_code:{country_code}"]
                    features.append(f"{country_code_feature}:1")

                if language_code is not None:
                    language_code_feature = feature_map[f"language_code:{language_code}"]
                    features.append(f"{language_code_feature}:1")

                author_id_feature = feature_map[f"author_id:{author_id}"]
                features.append(f"{author_id_feature}:1")

                for genre_id in genre_ids:
                    if genre_id is not None:
                        genre_id_feature = feature_map[f"genre_id:{genre_id}"]
                        features.append(f"{genre_id_feature}:1")

                # Numerical features
                if average_rating is not None:
                    average_rating_feature = feature_map["average_rating"]
                    features.append(f"{average_rating_feature}:{average_rating}")

                if ratings_count is not None:
                    ratings_count_feature = feature_map["ratings_count"]
                    features.append(f"{ratings_count_feature}:{ratings_count}")

                if num_pages is not None:
                    num_pages_feature = feature_map["num_pages"]
                    features.append(f"{num_pages_feature}:{num_pages}")

                if publication_year is not None:
                    publication_year_feature = feature_map["publication_year"]
                    features.append(f"{publication_year_feature}:{int(publication_year)}")

                # Build line string
                line = str(label) + " " + " ".join(features) + "\n"
                
                # Split into sets and write line to set
                split = random.random()
                if split < train_threshold:
                    train_file.write(line)
                elif split < validation_threshold:
                    validation_file.write(line)
                else:
                    test_file.write(line)

            cursor.close()

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--host", required=True)
    parser.add_argument("--port", type=int, default=5432)
    parser.add_argument("--dbname", required=True)
    parser.add_argument("--user", required=True)
    parser.add_argument("--password", required=True)
    parser.add_argument("--train_ratio", type=float, required=True)
    parser.add_argument("--validation_ratio", type=float, required=True)
    args = parser.parse_args()

    main(args.host, args.port, args.dbname, args.user, args.password, args.train_ratio, args.validation_ratio)