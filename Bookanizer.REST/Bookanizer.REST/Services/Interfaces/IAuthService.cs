using Bookanizer.REST.API.DTOs;

namespace Bookanizer.REST.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto req, CancellationToken ct);
        Task<string> LoginAsync(LoginRequestDto req,  CancellationToken ct);
    }
}
