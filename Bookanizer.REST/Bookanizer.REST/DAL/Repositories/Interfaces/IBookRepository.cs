using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task CreateOrUpdateSingleAsync(BookModel book, CancellationToken ct = default);
        Task<BookModel?> ReadSingleByIdAsync(int bookId, CancellationToken ct = default);
        Task<List<BookModel>> ReadMultipleByTitleAsync(string title, int skip, int take, CancellationToken ct = default);
        Task<List<BookModel>> ReadMultipleByTitleWithoutSeriesAsync(string titleWithoutSeries, int skip, int take, CancellationToken ct = default);
        Task<bool> DeleteSingleByIdAsync(int bookId, CancellationToken ct = default);
    }
}
