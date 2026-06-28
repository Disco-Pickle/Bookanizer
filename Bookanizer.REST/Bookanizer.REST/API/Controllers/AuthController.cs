using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        #region Services
        private readonly IAuthService _authSvc;
        #endregion

        #region Constructors
        public AuthController(IAuthService authSvc)
        {
            _authSvc = authSvc;
        }
        #endregion

        #region Endpoints
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct = default)
        {
            try
            {
                await _authSvc.RegisterAsync(req, ct);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Registration failed.");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct = default)
        {
            try
            {
                return Ok(await _authSvc.LoginAsync(req, ct));
            }
            catch (InvalidOperationException)
            {
                return Unauthorized("Invalid credentials.");
            }
        }
        #endregion
    }
}
