using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwt(UserModel user);
    }
}
