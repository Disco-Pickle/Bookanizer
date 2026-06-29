using Bookanizer.REST.API.DTOs;

namespace Bookanizer.REST.Services.Interfaces
{
    public interface IInteractionService
    {
        Task<IEnumerable<InteractionReadDto>> ReadAsync(string userId, CancellationToken ct);
        Task UpsertAsync(string userId, InteractionUpsertDto interaction, CancellationToken ct);
        Task DeleteAsync(string userId, int bookId, CancellationToken ct);
    }
}
