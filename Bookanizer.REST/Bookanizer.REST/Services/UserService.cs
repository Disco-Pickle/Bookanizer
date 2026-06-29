using Bookanizer.REST.API.DTOs;
using Bookanizer.REST.Services.Interfaces;

namespace Bookanizer.REST.Services
{
    public class UserService : IUserService
    {
        #region Repositories
        #endregion

        #region Constructors
        public UserService() { }
        #endregion

        #region Methods
        public Task<UserReadDto> ReadAsync(string userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
