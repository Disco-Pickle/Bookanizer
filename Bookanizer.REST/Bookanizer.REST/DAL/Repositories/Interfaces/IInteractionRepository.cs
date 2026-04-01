using Bookanizer.REST.DAL.Models;

namespace Bookanizer.REST.DAL.Repositories.Interfaces
{
    public interface IInteractionRepository
    {
        #region User Level CRUD Operations
        Task CreateOrUpdateSingleAsync(InteractionModel interaction, CancellationToken ct = default);
        Task<InteractionModel?> ReadSingleByUserIdAndBookIdAsync(string userId, int bookId, CancellationToken ct = default);
        Task<List<InteractionModel>> ReadMultipleByUserIdAsync(string userId, int skip, int take, CancellationToken ct = default);
        Task<bool> DeleteSingleByUserIdAndBookIdAsync(string userId, int bookId, CancellationToken ct = default);
        Task<bool> DeleteMultipleByUserIdAndBookIdsAsync(string userId, List<int> bookIds, CancellationToken ct = default);
        #endregion

        #region Admin Level CRUD Operations
        Task DeleteAllAsync(CancellationToken ct = default);
        #endregion
    }
}
