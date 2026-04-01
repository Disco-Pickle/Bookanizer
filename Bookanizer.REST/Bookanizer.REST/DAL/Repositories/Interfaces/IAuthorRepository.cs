using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task CreateOrUpdateSingleAsync(AuthorModel author, CancellationToken ct = default);
        Task<AuthorModel?> ReadSingleByIdAsync(int authorId, CancellationToken ct = default);
        Task<List<AuthorModel>> ReadMultipleByNameAsync(string name, int skip, int take, CancellationToken ct = default);
        Task<bool> DeleteSingleByIdAsync(int authorId, CancellationToken ct = default);
    }
}
