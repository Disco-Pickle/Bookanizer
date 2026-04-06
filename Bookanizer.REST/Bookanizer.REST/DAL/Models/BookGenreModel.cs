using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Models
{
    [Table("book_genres")]
    [PrimaryKey("BookId", "GenreId")]
    public class BookGenreModel
    {
        #region Constructors
        public BookGenreModel()
        {
            BookId = 0;
            Book = null!; // Tells the compiler I know it looks like null, but EF Core will populate it
            GenreId = 0;
            Genre = null!; // Tells the compiler I know it looks like null, but EF Core will populate it
        }

        public BookGenreModel(
            int bookId,
            int genreId)
        {
            BookId = bookId;
            Book = null!; // EF Core will also resolve the Book for the parameterized constructor
            GenreId = genreId;
            Genre = null!; // EF Core will also resolve the Genre for the parameterized constructor
        }
        #endregion

        #region Properties
        [Column("book_id")]
        public int BookId { get; set; }

        public BookModel Book { get; set; }

        [Column("genre_id")]
        public int GenreId { get; set; }

        public GenreModel Genre { get; set; }
        #endregion

        // No Update method: book_genres is a pure relational table, no updating should be performed, if relations change, the old relation should be deleted and a new one created
    }
}
