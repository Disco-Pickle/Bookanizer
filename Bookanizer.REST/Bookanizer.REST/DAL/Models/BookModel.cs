using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookanizer.REST.DAL.Models
{
    [Table("books")]
    public class BookModel
    {
        #region Constructor
        public BookModel() 
        { 
            Id = Guid.NewGuid();
            Isbn = null;
            Isbn13 = null;
            CountryCode = null; 
            LanguageCode =  null;
            AverageRating = 0.0;
            RatingsCount = 0;
            Author = null!; // Tells the compiler I know it looks like null, but EF will populate it
            AuthorId = Guid.Empty;
            NumPages = 0;
            PublicationDate = null;
            Title = null;
            TitleWithoutSeries = string.Empty;
        }
        #endregion

        #region Properties
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [MaxLength(10)]
        [Column("isbn")]
        public string? Isbn { get; set; }

        [Column("isbn13")]
        [MaxLength(13)]
        public string? Isbn13 { get; set; }

        [Column("country_code")]
        [MaxLength(255)]
        public string? CountryCode { get; set; }
        
        [Column("language_code")]
        [MaxLength(255)]
        public string? LanguageCode { get; set; }

        [Column("average_rating")]
        public double AverageRating { get; set; }

        [Column("ratings_count")]
        public int RatingsCount { get; set; }

        [Required]
        [Column("author_id")]
        public Guid AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        [Column("num_pages")]
        public int NumPages { get; set; }

        [Column("publication_date")]
        public DateOnly? PublicationDate { get; set; }

        [Column("title")]
        [MaxLength(511)]
        public string? Title { get; set; }

        [Required]
        [Column("title_without_series")]
        [MaxLength(255)]
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
            Guid authorId, 
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
