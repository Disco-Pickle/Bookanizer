using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
            AverageRating = 0.0;
            RatingsCount = 0;
            Author = null!; // Tells the compiler I know it looks like null, but EF Core will populate it
            AuthorId = 0;
            NumPages = 0;
            PublicationDate = null;
            Title = null;
            TitleWithoutSeries = string.Empty;
        }
        public BookModel(
            int bookId,
            string? isbn,
            string? isbn13,
            string? countryCode,
            string? languageCode,
            double averageRating,
            int ratingsCount,
            int authorId,
            int numPages,
            DateOnly? publicationDate,
            string? title,
            string titleWithoutSeries)
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
        public double AverageRating { get; set; }

        [Column("ratings_count")]
        public int RatingsCount { get; set; }

        [Column("author_id")]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public AuthorModel Author { get; set; }

        [Column("num_pages")]
        public int NumPages { get; set; }

        [Column("publication_date")]
        public DateOnly? PublicationDate { get; set; }

        [Column("title")]
        [MaxLength(512)]
        public string? Title { get; set; }

        [Column("title_without_series")]
        [Required]
        [MaxLength(256)]
        public string TitleWithoutSeries { get; set; }
        #endregion

        #region Methods
        public void Update(
            string? isbn, 
            string? isbn13, 
            string? countryCode, 
            string? languageCode, 
            double averageRating, 
            int ratingsCount, 
            int authorId, 
            int numPages, 
            DateOnly? publicationDate,
            string? title, 
            string titleWithoutSeries)
        {
            Isbn = isbn;
            Isbn13 = isbn13;
            CountryCode = countryCode;
            LanguageCode = languageCode;
            AverageRating = averageRating;
            RatingsCount = ratingsCount;
            AuthorId = authorId;
            NumPages = numPages;
            PublicationDate = publicationDate;
            Title = title;
            TitleWithoutSeries = titleWithoutSeries;
        }
        #endregion
    }
}
