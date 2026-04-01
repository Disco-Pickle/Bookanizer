using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Constructors
        public UserRepository(DataContext db)
        {
            _db = db;
        }
        #endregion

        #region Data Context
        private readonly DataContext _db;
        #endregion

        #region CRUD
        public async Task CreateSingleAsync(
            UserModel user,
            CancellationToken ct = default)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId, ct);
            if (dbUser == null)
            {
                await _db.Users.AddAsync(user, ct);
                await _db.SaveChangesAsync(ct);
            }
            else
            {
                throw new InvalidOperationException("Cannot create user, user with this ID already exists.");
            }
        }

        public async Task<UserModel?> ReadSingleByIdAsync(
            string userId,
            CancellationToken ct = default)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId, ct);
        }

        public async Task UpdateSingleAsync(
            UserModel user,
            CancellationToken ct = default)
        {
            var dbUser = await _db.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId, ct);
            if (dbUser != null)
            {
                dbUser.Update(user);
                await _db.SaveChangesAsync(ct);
            }
            else
            {
                throw new InvalidOperationException("Cannot update user, not found");
            }
        }

        public async Task<bool> DeleteSingleByIdAsync(
            string id,
            CancellationToken ct = default)
        {
            var amountDeleted = await _db.Users.Where(u => u.UserId == id)
                                               .ExecuteDeleteAsync(ct); // ExecuteDeleteAsync does not need a SaveChanges() call

            return amountDeleted > 0;
        }
        #endregion
    }
}
