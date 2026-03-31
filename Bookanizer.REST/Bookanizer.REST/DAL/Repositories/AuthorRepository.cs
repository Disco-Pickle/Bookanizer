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
            var found = await _db.Authors.FirstOrDefaultAsync(dbAuthor => dbAuthor.AuthorId == author.AuthorId);
            if (found == null)
            {
                await _db.Authors.AddAsync(author);
            }
            else
            {
                found.Update(author.Name);
            }
            await _db.SaveChangesAsync(ct);
        }

        public async Task<AuthorModel?> ReadSingleByIdAsync(
            int id, 
            CancellationToken ct = default)
        {
            return await _db.Authors.AsNoTracking().FirstOrDefaultAsync(dbAuthor => dbAuthor.AuthorId == id);
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
                queryable = queryable.Where(author => EF.Functions.ILike(author.Name, $"%{name}%")); // Case-insensitive search for author's name
            }

            return await queryable.OrderByDescending(author => author.Name)
                                  .ThenByDescending(author => author.AuthorId)
                                  .Skip(skip)
                                  .Take(take)
                                  .ToListAsync(ct);
        }

        public async Task<List<AuthorModel>> ReadAllAsync(
            int skip, 
            int take, 
            CancellationToken ct = default)
        {
            return await _db.Authors.AsNoTracking()   // AsNoTracking to improve performance (no unnecessary snapshots)
                                    .OrderByDescending(author => author.Name)
                                    .ThenByDescending(author => author.AuthorId)
                                    .Skip(skip)       // Skip to allow pagination (allows guarding against pulling entire db)
                                    .Take(take)       // Take to allow pagination (allows guarding against pulling entire db)
                                    .ToListAsync(ct); 
        }

        public async Task<bool> DeleteSingleByIdAsync(int id, CancellationToken ct = default)
        {
            var deleted = await _db.Authors.Where(dbAuthor => dbAuthor.AuthorId == id)
                                           .ExecuteDeleteAsync(ct); // ExecuteDeleteAsync does not need a SaveChanges() call
            return deleted > 0;
        }

        public async Task DeleteAllAsync(CancellationToken ct = default)
        {
           await _db.Authors.ExecuteDeleteAsync(ct);
        }
        #endregion
    }
}
