using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bookanizer.REST.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Models
{
    [Table("interactions")]
    [PrimaryKey("UserId", "BookId")]
    public class InteractionModel
    {
        #region Constructors
        public InteractionModel()
        {
            UserId = string.Empty;
            User = null!; // Tells the compiler I know it looks like null, but EF Core will populate it
            BookId = 0;
            Book = null!; // Tells the compiler I know it looks like null, but EF Core will populate it
            IsRead = false;
            Rating = 0.0;
            DateAdded = null;
            DateUpdated = null;
            ReadAt = null;
            StartedAt = null;
            ReadLocation = ReadLocationEnum.None;
        }
        public InteractionModel(
            string userId,
            int bookId,
            bool isRead,
            double rating,
            DateTimeOffset? dateAdded,
            DateTimeOffset? dateUpdated,
            DateTimeOffset? readAt,
            DateTimeOffset? startedAt,
            ReadLocationEnum? readLocation)
        {
            UserId = userId;
            User = null!; // EF Core will also resolve the User for the parameterized constructor
            BookId = bookId;
            Book = null!; // EF Core will also resolve the Book for the parameterized constructor
            IsRead = isRead;
            Rating = rating;
            DateAdded = dateAdded;
            DateUpdated = dateUpdated;
            ReadAt = readAt;
            StartedAt = startedAt;
            ReadLocation = readLocation;
        }
        #endregion

        #region Properties
        [Column("user_id")]
        [MaxLength(32)]
        public string UserId { get; set; } // MD5 hash in the Wan & McAuley Goodreads dataset

        public UserModel User { get; set; }

        [Column("book_id")]
        public int BookId { get; set; }

        public BookModel Book { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("rating")]
        public double Rating { get; set; }

        [Column("date_added")]
        public DateTimeOffset? DateAdded { get; set; }

        [Column("date_updated")]
        public DateTimeOffset? DateUpdated {  get; set; }

        [Column("read_at")]
        public DateTimeOffset? ReadAt { get; set; }

        [Column("started_at")]
        public DateTimeOffset? StartedAt { get; set; }

        [Column("read_location")]
        public ReadLocationEnum? ReadLocation { get; set; }
        #endregion

        #region Methods
        public void Update(InteractionModel interaction)
        {
            IsRead = interaction.IsRead;
            Rating = interaction.Rating;
            DateAdded = interaction.DateAdded;
            DateUpdated = interaction.DateUpdated;
            ReadAt = interaction.ReadAt;
            StartedAt = interaction.StartedAt;
            ReadLocation = interaction.ReadLocation;
        }
        #endregion
    }
}
