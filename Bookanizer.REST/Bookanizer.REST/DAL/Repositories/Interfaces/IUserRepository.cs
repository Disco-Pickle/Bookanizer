using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task CreateSingleAsync(UserModel user, CancellationToken ct = default);
        Task<UserModel?> ReadSingleByIdAsync(string userId, CancellationToken ct = default);
        Task UpdateSingleAsync(UserModel user, CancellationToken ct = default);
        Task<bool> DeleteSingleByIdAsync(string id, CancellationToken ct = default);
    }
}
