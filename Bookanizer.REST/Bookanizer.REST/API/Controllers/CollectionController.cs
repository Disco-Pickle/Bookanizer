using System.Security.Claims;
using Bookanizer.REST.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CollectionController : ControllerBase
    {
        #region Services
        private readonly IInteractionService _interactionSvc;
        #endregion

        #region Constructors
        public CollectionController(IInteractionService interactionSvc)
        {
            _interactionSvc = interactionSvc;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InteractionReadDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            // Authorization via JWT
            string? userId = GetUserIdOrNull();
            if (userId is null) { return Unauthorized(); } // Fallback, should not be reached if Authorization worked properly

            // Getting collection of interactions
            var collection = await _interactionSvc.ReadAsync(userId, ct);
            if (collection is null) { throw new InvalidOperationException("Collection GET returned null unexpectedly."); }
            return Ok(collection);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Put(InteractionUpsertDto interaction, CancellationToken ct = default)
        {
            // Authorize via JWT
            string? userId = GetUserIdOrNull();
            if (userId is null) { return Unauthorized(); } // Fallback, should not be reached if Authorization worked properly

            // Upserting interaction
            await _interactionSvc.UpsertAsync(userId, interaction, ct);
            return NoContent();
        }

        [HttpDelete("{bookId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int bookId, CancellationToken ct = default)
        {
            // Authorize via JWT
            string? userId = GetUserIdOrNull();
            if (userId is null) { return Unauthorized(); } // Fallback, should not be reached if Authorization worked properly

            // Deleting interaction
            await _interactionSvc.DeleteAsync(userId, bookId, ct);
            return NoContent();
        }
        #endregion

        #region Helpers
        private string? GetUserIdOrNull()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier); // or User.FindFirstValue(JwtRegisteredClaimNames.Sub) if options.MapInboundClaims = false is set in authService
            return string.IsNullOrWhiteSpace(sub) ? null : sub;
        }
        #endregion
    }
}
