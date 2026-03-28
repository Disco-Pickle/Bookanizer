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
            Books = new List<BookModel>();
        }
        public AuthorModel(
            int authorId,
            string name)
        {
            AuthorId = authorId;
            Name = name;
            Books = new List<BookModel>();
        }
        #endregion

        #region Properties
        [Column("author_id")]
        [Key]
        public int AuthorId { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        public ICollection<BookModel> Books { get; set; }
        #endregion

        #region Methods 
        public  void Update(
            string name)
        {
            Name = name;
        }
        #endregion
    }
}
