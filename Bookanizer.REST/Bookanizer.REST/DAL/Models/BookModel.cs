using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookanizer.REST.DAL.Models
{
    [Table("books")]
    public class BookModel
    {
        #region Constructors
        public BookModel() 
        { 
            BookId = 0;
            Isbn = null;
            Isbn13 = null;
            CountryCode = null; 
            LanguageCode =  null;
            AverageRating = null;
            RatingsCount = null;
            Author = null!; // Tells the compiler I know it looks like null, but EF Core will populate it
            AuthorId = 0;
            NumPages = null;
            PublicationDate = null;
            Title = null;
            TitleWithoutSeries = null;
            Interactions = new List<InteractionModel>();
            BookGenres = new List<BookGenreModel>();
        }
        public BookModel(
            int bookId,
            string? isbn,
            string? isbn13,
            string? countryCode,
            string? languageCode,
            double? averageRating,
            int? ratingsCount,
            int authorId,
            int? numPages,
            DateOnly? publicationDate,
            string? title,
            string? titleWithoutSeries)
        {
            BookId = bookId;
            Isbn = isbn;
            Isbn13 = isbn13;
            CountryCode = countryCode;
            LanguageCode = languageCode;
            AverageRating = averageRating;
            RatingsCount = ratingsCount;
            Author = null!; // EF Core will also resolve the Author for the parameterized constructor
            AuthorId = authorId;
            NumPages = numPages;
            PublicationDate = publicationDate;
            Title = title;
            TitleWithoutSeries = titleWithoutSeries;
            Interactions = new List<InteractionModel>();
            BookGenres = new List<BookGenreModel>();
        }
        #endregion

        #region Properties
        [Column("book_id")]
        [Key]
        public int BookId { get; set; }

        [Column("isbn")]
        [MaxLength(10)]
        public string? Isbn { get; set; }

        [Column("isbn13")]
        [MaxLength(13)]
        public string? Isbn13 { get; set; }

        [Column("country_code")]
        [MaxLength(256)]
        public string? CountryCode { get; set; }
        
        [Column("language_code")]
        [MaxLength(256)]
        public string? LanguageCode { get; set; }

        [Column("average_rating")]
        public double? AverageRating { get; set; }

        [Column("ratings_count")]
        public int? RatingsCount { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }

        public AuthorModel Author { get; set; }

        [Column("num_pages")]
        public int? NumPages { get; set; }

        [Column("publication_date")]
        public DateOnly? PublicationDate { get; set; }

        [Column("title")]
        [MaxLength(512)]
        public string? Title { get; set; }

        [Column("title_without_series")]
        [MaxLength(256)]
        public string? TitleWithoutSeries { get; set; }

        public ICollection<InteractionModel> Interactions { get; set; }
        
        public ICollection<BookGenreModel> BookGenres { get; set; }
        #endregion

        #region Methods
        public void Update(BookModel book)
        {
            Isbn = book.Isbn;
            Isbn13 = book.Isbn13;
            CountryCode = book.CountryCode;
            LanguageCode = book.LanguageCode;
            AverageRating = book.AverageRating;
            RatingsCount = book.RatingsCount;
            AuthorId = book.AuthorId;
            NumPages = book.NumPages;
            PublicationDate = book.PublicationDate;
            Title = book.Title;
            TitleWithoutSeries = book.TitleWithoutSeries;
        }
        #endregion
    }
}
