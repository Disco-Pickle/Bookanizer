using Bookanizer.REST.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL
{
    public class DataContext : DbContext
    {
        #region Constructor
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        #endregion

        #region DbSets (Tables)
        public DbSet<AuthorModel> Authors { get; set; }
        public DbSet<BookGenreModel> BookGenres { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<InteractionModel> Interactions { get; set; }
        public DbSet<UserModel> Users { get; set; }
        #endregion

        #region Builder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookModel>()
                .HasOne<AuthorModel>(book => book.Author) // Books have one author
                .WithMany(author => author.Books)         // Authors have many books
                .HasForeignKey(book => book.AuthorId)     // FK: AuthorId
                .OnDelete(DeleteBehavior.Cascade);        // On deletion of an author, delete dependent books
            modelBuilder.Entity<InteractionModel>()
                .HasOne<UserModel>(interaction => interaction.User) // Interactions have one user
                .WithMany(user => user.Interactions)                // Users have many interactions
                .HasForeignKey(interaction => interaction.UserId)   // FK: UserId
                .OnDelete(DeleteBehavior.Cascade);                  // On deletion of a user, delete dependent interactions
            modelBuilder.Entity<InteractionModel>()
                .HasOne<BookModel>(interaction => interaction.Book) // Interactions have one book
                .WithMany(book => book.Interactions)                // Books have many interactions
                .HasForeignKey(interaction => interaction.BookId)   // FK: BookId
                .OnDelete(DeleteBehavior.Cascade);                  // On deletion of a book, delete dependent interactions
            modelBuilder.Entity<BookGenreModel>()
                .HasOne<BookModel>(bookGenre => bookGenre.Book) // BookGenre relations have one book
                .WithMany(book => book.BookGenres)              // Books have many bookGenre relations
                .HasForeignKey(bookGenre => bookGenre.BookId)   // FK: BookId
                .OnDelete(DeleteBehavior.Cascade);              // On deletion of a book, delete dependent bookGenre relations
            modelBuilder.Entity<BookGenreModel>()
                .HasOne<GenreModel>(bookGenre => bookGenre.Genre) // BookGenre relations have one genre
                .WithMany(genre => genre.BookGenres)              // Genres have many bookGenre relations
                .HasForeignKey(bookGenre => bookGenre.GenreId)    // FK: GenreId
                .OnDelete(DeleteBehavior.Cascade);                // On deletion of a genre, delete dependent bookGenre relations
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
