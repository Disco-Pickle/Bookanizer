using Bookanizer.REST.API.DTOs;

namespace Bookanizer.REST.Services.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookReadDto>> SearchAsync(string query, CancellationToken ct);
    }
}
