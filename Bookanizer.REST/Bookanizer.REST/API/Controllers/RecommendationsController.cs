using System.Security.Claims;
using Bookanizer.REST.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RecommendationsController : ControllerBase
    {
        #region Services
        private readonly IRecommendationService _recommendationSvc;
        #endregion

        #region Constructors
        public RecommendationsController(IRecommendationService recommendationSvc) 
        { 
            _recommendationSvc = recommendationSvc;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        [ProducesResponseType(typeof(BookReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            // Authorization via JWT
            string? userId = GetUserIdOrNull();
            if (userId is null) { return Unauthorized(); } // Fallback, should not be reached if Authorization worked properly

            // Get recommendation
            var recommendation = await _recommendationSvc.GetRecommendationAsync(userId, ct);
            return Ok(recommendation);
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
