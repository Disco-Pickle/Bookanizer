using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        #region User Level CRUD Operations
        Task<AuthorModel?> ReadSingleByIdAsync(int authorId, CancellationToken ct = default);
        Task<List<AuthorModel>> ReadMultipleByNameAsync(string name, int skip, int take, CancellationToken ct = default);
        #endregion

        #region Admin Level CRUD Operations
        Task CreateOrUpdateSingleAsync(AuthorModel author, CancellationToken ct = default);
        Task<bool> DeleteSingleByIdAsync(int authorId, CancellationToken ct = default);
        Task DeleteAllAsync(CancellationToken ct = default);
        #endregion
    }
}
