using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        #region Constructors
        public AuthorRepository(DataContext db)
        {
            _db = db;
        }
        #endregion

        #region Data Context
        private readonly DataContext _db;
        #endregion

        #region CRUD
        public async Task CreateOrUpdateSingleAsync(
            AuthorModel author, 
            CancellationToken ct = default) // CTs so user etc cancellations lead to cancelled data requests etc
        {
            var dbAuthor = await _db.Authors.FirstOrDefaultAsync(a => a.AuthorId == author.AuthorId, ct);
            if (dbAuthor == null)
            {
                await _db.Authors.AddAsync(author, ct);
            }
            else
            {
                dbAuthor.Update(author);
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task<AuthorModel?> ReadSingleByIdAsync(
            int id, 
            CancellationToken ct = default)
        {
            return await _db.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.AuthorId == id, ct);
        }


        public async Task<List<AuthorModel>> ReadMultipleByNameAsync(
            string name, 
            int skip, 
            int take, 
            CancellationToken ct = default)
        {
            IQueryable<AuthorModel> queryable = _db.Authors.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                queryable = queryable.Where(a => EF.Functions.ILike(a.Name, $"%{name}%")); // Case-insensitive search for author's name
            }

            return await queryable.OrderByDescending(a => a.Name)
                                  .ThenByDescending(a => a.AuthorId)
                                  .Skip(skip)
                                  .Take(take)
                                  .ToListAsync(ct);
        }

        public async Task<bool> DeleteSingleByIdAsync(
            int id, 
            CancellationToken ct = default)
        {
            var amountDeleted = await _db.Authors.Where(a => a.AuthorId == id)
                                                 .ExecuteDeleteAsync(ct); // ExecuteDeleteAsync does not need a SaveChanges() call
            
            return amountDeleted > 0;
        }
        #endregion
    }
}
