using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookanizer.REST.DAL.Models
{
    [Table("users")]
    public class UserModel
    {
        #region Constructors
        public UserModel()
        {
            UserId = string.Empty;
            Username = string.Empty;
            PasswordHash = string.Empty;
            Interactions = new List<InteractionModel>();
        }
        public UserModel(
            string userId,
            string username,
            string passwordHash)
        {
            UserId = userId;
            Username = username;
            PasswordHash = passwordHash;
            Interactions = new List<InteractionModel>();
        }
        #endregion

        #region Properties
        [Column("user_id")]
        [Key]
        [MaxLength(32)]
        public string UserId { get; set; } // Will be an MD5 hash

        [Column("username")]
        [Required]
        [MaxLength(64)]
        public string Username { get; set; }

        [Column("password_hash")]
        [Required]
        [MaxLength(512)]
        public string PasswordHash { get; set; }

        public ICollection<InteractionModel> Interactions { get; set; }
        #endregion

        #region Methods
        public void Update(
            string username,
            string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
        #endregion
    }
}
