using Bookanizer.REST.API.DTOs;
using Bookanizer.REST.Services.Interfaces;

namespace Bookanizer.REST.Services
{
    public class RecommendationService : IRecommendationService
    {
        #region Repositories
        #endregion

        #region Constructors
        public RecommendationService() { }
        #endregion

        #region Methods
        public Task<BookReadDto> GetRecommendationAsync(string userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
