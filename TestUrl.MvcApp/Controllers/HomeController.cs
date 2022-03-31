using Microsoft.AspNetCore.Mvc;
using TestUrls.BusinessLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly BusinessService _businessServer;

        public HomeController(BusinessService businessServer)
        {
            _businessServer = businessServer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var testedLinks = _businessServer.GetTestedLinks();

            return View(testedLinks);
        }

        [HttpPost]
        public IActionResult Index([FromBody] string link)
        {
            return Content(link);
        }
    }
}
