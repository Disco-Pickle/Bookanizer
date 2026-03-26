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
        public DbSet<BookModel> Books { get; set; }
        public DbSet<InteractionModel> Interactions { get; set; }
        public DbSet<UserModel> Users { get; set; }
        #endregion

        #region Builder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookModel>()
                .HasOne<AuthorModel>()                  // Books have one author
                .WithMany()                             // Authors have many books
                .HasForeignKey(book => book.AuthorId)   // FK: AuthorId
                .OnDelete(DeleteBehavior.Cascade);      // On deletion of an author, delete dependent books
            modelBuilder.Entity<InteractionModel>()
                .HasOne<UserModel>()                                // Interactions have one user
                .WithMany()                                         // Users have many interactions
                .HasForeignKey(interaction => interaction.UserId)   // FK: UserId
                .OnDelete(DeleteBehavior.Cascade);                  // On deletion of a user, delete dependent interactions
            modelBuilder.Entity<InteractionModel>()
                .HasOne<BookModel>()                                // Interactions have one book
                .WithMany()                                         // Books have many interactions
                .HasForeignKey(interaction => interaction.BookId)   // FK: BookId
                .OnDelete(DeleteBehavior.Cascade);                  // On deletion of a book, delete dependent interactions
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
