using System.Security.Claims;
using Bookanizer.REST.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        #region Services
        private readonly IUserService _userSvc;
        #endregion

        #region Constructors
        public UsersController(IUserService userSvc)
        {
            _userSvc = userSvc;
        }
        #endregion

        #region Endpoints
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserReadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(CancellationToken ct = default)
        {
            // Authorization via JWT
            string? userId = GetUserIdOrNull();
            if (userId is null) { return Unauthorized(); } // Fallback, should not be reached if Authorization worked properly
            
            // Getting user data
            var user = await _userSvc.ReadAsync(userId, ct);
            return user is null ? NotFound() : Ok(user);
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
