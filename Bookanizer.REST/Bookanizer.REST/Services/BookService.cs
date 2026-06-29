using Bookanizer.REST.API.DTOs;
using Bookanizer.REST.Services.Interfaces;

namespace Bookanizer.REST.Services
{
    public class BookService : IBookService
    {
        #region Repositories
        #endregion

        #region Constructors
        public BookService() { }
        #endregion

        #region Methods
        public Task<IEnumerable<BookReadDto>> SearchAsync(string query, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
