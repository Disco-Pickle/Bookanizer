using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Repositories
{
    public class InteractionRepository : IInteractionRepository
    {
        #region Constructors
        public InteractionRepository(DataContext db)
        {
            _db = db;
        }
        #endregion

        #region Data Context
        private readonly DataContext _db;
        #endregion

        #region User Level CRUD Operations
        public async Task CreateOrUpdateSingleAsync(
            InteractionModel interaction,
            CancellationToken ct = default)
        {
            var dbInteraction = await _db.Interactions.FirstOrDefaultAsync(i => i.UserId == interaction.UserId && i.BookId == interaction.BookId, ct); // Composite key is looked up
            if (dbInteraction == null)
            {
                await _db.Interactions.AddAsync(interaction, ct);
            }
            else
            {
                dbInteraction.Update(interaction);
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task<InteractionModel?> ReadSingleByUserIdAndBookIdAsync(
            string userId,
            int bookId,
            CancellationToken ct = default)
        {
            return await _db.Interactions.AsNoTracking()
                                         .FirstOrDefaultAsync(i => i.UserId == userId && i.BookId == bookId, ct);
        }

        public async Task<List<InteractionModel>> ReadMultipleByUserIdAsync(
            string userId,
            int skip,
            int take,
            CancellationToken ct = default)
        {
            IQueryable<InteractionModel> queryable = _db.Interactions.AsNoTracking()
                                                                     .Where(i => i.UserId == userId)
                                                                     .Include(i => i.Book)
                                                                     .ThenInclude(b => b.Author);

            return await queryable.OrderBy(i => i.Book.TitleWithoutSeries)
                                  .ThenBy(i => i.Book.Author.Name)
                                  .ThenBy(i => i.BookId)
                                  .Skip(skip)
                                  .Take(take)
                                  .ToListAsync(ct);
        }

        public async Task<bool> DeleteSingleByUserIdAndBookIdAsync(
            string userId,
            int bookId,
            CancellationToken ct = default)
        {
            var amountDeleted = await _db.Interactions.Where(i => i.UserId == userId && i.BookId == bookId)
                                                      .ExecuteDeleteAsync(ct);
            return amountDeleted > 0;
        }

        public async Task<bool> DeleteMultipleByUserIdAndBookIdsAsync(
            string userId,
            List<int> bookIds,
            CancellationToken ct = default)
        {
            var amountDeleted = await _db.Interactions.Where(i => i.UserId == userId && bookIds.Contains(i.BookId))
                                                      .ExecuteDeleteAsync(ct);

            return amountDeleted > 0;
        }
        #endregion

        #region Admin Level CRUD Operations
        public async Task DeleteAllAsync(
            CancellationToken ct = default)
        {
            await _db.Interactions.ExecuteDeleteAsync(ct);
        }
        #endregion
    }
}
