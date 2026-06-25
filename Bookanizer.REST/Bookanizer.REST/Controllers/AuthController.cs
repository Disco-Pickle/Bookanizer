using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
        {
            try
            {
                await _auth.RegisterAsync(req, ct);
                return NoContent();
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Registration failed.");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req, CancellationToken ct)
        {
            try
            {
                return Ok(await _auth.LoginAsync(req, ct));
            }
            catch (InvalidOperationException)
            {
                return Unauthorized("Invalid credentials.");
            }
        }
    }
}
