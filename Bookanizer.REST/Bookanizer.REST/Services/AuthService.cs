using Bookanizer.REST.API.DTOs;
using Bookanizer.REST.Services.Interfaces;

namespace Bookanizer.REST.Services
{
    public class AuthService : IAuthService
    {
        #region Repositories
        #endregion

        #region Constructors
        public AuthService() { }
        #endregion

        #region Methods
        public Task RegisterAsync(RegisterRequestDto req, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoginAsync(LoginRequestDto req, CancellationToken ct)
        {
            throw new NotImplementedException(); // Must return login token
        }
        #endregion
    }
}
