using Bookanizer.REST.API.DTOs;
using Bookanizer.REST.Services.Interfaces;

namespace Bookanizer.REST.Services
{
    public class InteractionService : IInteractionService
    {
        #region Repositories
        #endregion

        #region Constructors
        public InteractionService() { }
        #endregion

        #region Methods
        public Task<IEnumerable<InteractionReadDto>> ReadAsync(string userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task UpsertAsync(string userId, InteractionUpsertDto interaction, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(string userId, int bookId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
