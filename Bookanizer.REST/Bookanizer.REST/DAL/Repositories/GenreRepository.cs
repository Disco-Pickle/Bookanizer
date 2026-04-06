using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        #region Constructors
        public GenreRepository(DataContext db)
        {
            _db = db;
        }
        #endregion

        #region Data Context
        private readonly DataContext _db;
        #endregion

        #region Admin Level CRUD Operations
        public async Task CreateOrUpdateSingleAsync(
            GenreModel genre,
            CancellationToken ct = default)
        {
            var dbGenre = await _db.Genres.FirstOrDefaultAsync(g => g.GenreId == genre.GenreId, ct);
            if (dbGenre == null)
            {
                await _db.Genres.AddAsync(genre, ct);
            }
            else
            {
                dbGenre.Update(genre);
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteSingleByIdAsync(
            int genreId,
            CancellationToken ct = default)
        {
            var amountDeleted = await _db.Genres.Where(g => g.GenreId == genreId)
                                                .ExecuteDeleteAsync(ct); // ExecuteDeleteAsync does not need a SaveChanges() call

            return amountDeleted > 0;
        }

        public async Task DeleteAllAsync(
            CancellationToken ct = default)
        {
            await _db.Genres.ExecuteDeleteAsync(ct);
        }
        #endregion
    }
}
