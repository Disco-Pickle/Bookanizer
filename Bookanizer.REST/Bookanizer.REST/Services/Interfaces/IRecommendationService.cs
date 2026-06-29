using Bookanizer.REST.API.DTOs;

namespace Bookanizer.REST.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<BookReadDto> GetRecommendationAsync(string userId, CancellationToken ct);
    }
}
