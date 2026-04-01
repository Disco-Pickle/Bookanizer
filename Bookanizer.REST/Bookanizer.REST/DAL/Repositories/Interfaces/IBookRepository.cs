using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IBookRepository
    {
        #region User Level CRUD Operations
        Task<BookModel?> ReadSingleByIdAsync(int bookId, CancellationToken ct = default);
        Task<List<BookModel>> ReadMultipleByTitleAsync(string title, int skip, int take, CancellationToken ct = default);
        Task<List<BookModel>> ReadMultipleByTitleWithoutSeriesAsync(string titleWithoutSeries, int skip, int take, CancellationToken ct = default);
        #endregion

        #region Admin Level CRUD Operations
        Task CreateOrUpdateSingleAsync(BookModel book, CancellationToken ct = default);
        Task<bool> DeleteSingleByIdAsync(int bookId, CancellationToken ct = default);
        Task DeleteAllAsync(CancellationToken ct = default);
        #endregion
    }
}
