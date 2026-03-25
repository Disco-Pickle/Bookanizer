using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookanizer.REST.DAL.Models
{
    [Table("authors")]
    public class AuthorModel
    {
        #region Constructors
        public AuthorModel()
        {
            AuthorId = 0;
            Name = string.Empty;
            AverageRating = 0.0;
            RatingsCount = 0;
            Books = new List<BookModel>();
        }
        public AuthorModel(
            int authorId,
            string name,
            double averageRating,
            int ratingsCount)
        {
            AuthorId = authorId;
            Name = name;
            AverageRating = averageRating;
            RatingsCount = ratingsCount;
            Books = new List<BookModel>();
        }
        #endregion

        #region Properties
        [Key]
        [Column("author_id")]
        public int AuthorId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Column("average_rating")]
        public double AverageRating { get; set; }

        [Column("ratings_count")]
        public int RatingsCount { get; set; }

        public ICollection<BookModel> Books { get; set; }
        #endregion

        #region Methods
        public  void Update(
            string name,
            double averageRating,
            int ratingsCount)
        {
            Name = name;
            AverageRating = averageRating;
            RatingsCount = ratingsCount;
        }
        #endregion
    }
}
