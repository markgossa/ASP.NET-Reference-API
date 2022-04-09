using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LSE.Stocks.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            return "Hello world!";
        }
    }
}
