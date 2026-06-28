using Bookanizer.REST.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        #region Services
        private readonly IBookService _bookSvc;
        #endregion

        #region Constructors
        public BooksController(IBookService bookSvc)
        {
            _bookSvc = bookSvc;
        }
        #endregion

        #region Endpoints
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<BookReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] string q, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(q)) { return BadRequest("Empty search not allowed."); }
            var results = await _bookSvc.SearchAsync(q, ct);
            if (results is null) { throw new InvalidOperationException("Book search returned null unexpectedly."); }
            return Ok(results);
        }
        #endregion
    }
}
