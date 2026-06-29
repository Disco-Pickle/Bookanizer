using Bookanizer.REST.API.DTOs;

namespace Bookanizer.REST.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserReadDto> ReadAsync(string userId, CancellationToken ct);
    }
}
