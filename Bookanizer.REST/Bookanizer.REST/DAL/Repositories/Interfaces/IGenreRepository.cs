using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        #region Admin Level CRUD Operations
        Task CreateOrUpdateSingleAsync(GenreModel genre, CancellationToken ct = default);
        Task<bool> DeleteSingleByIdAsync(int genreId, CancellationToken ct = default);
        Task DeleteAllAsync(CancellationToken ct = default);
        #endregion
    }
}
