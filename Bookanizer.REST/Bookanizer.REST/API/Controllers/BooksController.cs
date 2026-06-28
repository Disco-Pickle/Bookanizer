using Microsoft.AspNetCore.Mvc;

namespace Bookanizer.REST.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        public BooksController(ILogger<BooksController> logger)
        {
            
        }


    }
}
