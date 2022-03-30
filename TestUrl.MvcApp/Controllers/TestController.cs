using Microsoft.AspNetCore.Mvc;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
