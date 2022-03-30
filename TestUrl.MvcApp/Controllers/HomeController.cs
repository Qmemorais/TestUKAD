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
            var testLinks = _businessServer.GetTestedLinks();

            return View(testLinks);
        }

        [HttpPost]
        public IActionResult Index(string link)
        {
            return Content(link);
        }
    }
}
