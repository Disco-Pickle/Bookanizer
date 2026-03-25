using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Books : ControllerBase
    {
        private readonly ILogger<Books> _logger;

        public Books(ILogger<Books> logger)
        {
            _logger = logger;
        }
    }
}
