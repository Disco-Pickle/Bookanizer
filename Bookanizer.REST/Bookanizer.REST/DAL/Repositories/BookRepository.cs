using Bookanizer.REST.DAL.Models;
using Bookanizer.REST.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookanizer.REST.DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        #region Constructors
        public BookRepository(DataContext db)
        {
            _db = db;
        }
        #endregion

        #region Data Context
        private readonly DataContext _db;
        #endregion

        #region CRUD
        public async Task CreateOrUpdateSingleAsync(
            BookModel book,
            CancellationToken ct = default)
        {
            var dbBook = await _db.Books.FirstOrDefaultAsync(b => b.BookId == book.BookId, ct);
            if (dbBook == null)
            {
                await _db.Books.AddAsync(book, ct);
            }
            else
            {
                dbBook.Update(book);
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task<BookModel?> ReadSingleByIdAsync(
            int bookId,
            CancellationToken ct = default)
        {
            return await _db.Books.AsNoTracking().Include(b => b.Author).FirstOrDefaultAsync(b => b.BookId == bookId, ct);
        }

        public async Task<List<BookModel>> ReadMultipleByTitleAsync(
            string title,
            int skip,
            int take,
            CancellationToken ct = default)
        {
            IQueryable<BookModel> queryable = _db.Books.AsNoTracking().Include(b => b.Author);

            if (!string.IsNullOrWhiteSpace(title))
            {
                queryable = queryable.Where(b => EF.Functions.ILike(b.Title ?? "", $"%{title}%")); // Case-insensitive search for book's title
            }

            return await queryable.OrderByDescending(b => b.TitleWithoutSeries)
                                  .ThenByDescending(b => b.Author.Name)
                                  .ThenByDescending(b => b.BookId)
                                  .Skip(skip)
                                  .Take(take)
                                  .ToListAsync(ct);
        }

        public async Task<List<BookModel>> ReadMultipleByTitleWithoutSeriesAsync(
            string titleWithoutSeries,
            int skip,
            int take,
            CancellationToken ct = default)
        {
            IQueryable<BookModel> queryable = _db.Books.AsNoTracking().Include(b => b.Author);

            if (!string.IsNullOrWhiteSpace(titleWithoutSeries))
            {
                queryable = queryable.Where(b => EF.Functions.ILike(b.TitleWithoutSeries ?? "", $"%{titleWithoutSeries}%")); // Case-insensitive search for book's title without series
            }

            return await queryable.OrderByDescending(b => b.TitleWithoutSeries)
                                    .ThenByDescending(b => b.Author.Name)
                                    .ThenByDescending(b => b.BookId)
                                    .Skip(skip)
                                    .Take(take)
                                    .ToListAsync(ct);
        }

        public async Task<bool> DeleteSingleByIdAsync(
            int bookId, 
            CancellationToken ct = default)
        {
            var amountDeleted = await _db.Books.Where(b => b.BookId == bookId)
                                               .ExecuteDeleteAsync(ct); // ExecuteDeleteAsync does not need a SaveChanges() call

            return amountDeleted > 0;
        }
        #endregion
    }
}
