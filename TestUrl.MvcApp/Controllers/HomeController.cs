using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestUrl.MvcApp.Models;
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
        public IActionResult Index(int page = 1)
        {
            var testedLinks = _businessServer.GetTestedLinks().ToList();
            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                TotalItems = testedLinks.Count
            };
            var linksOnPage = testedLinks.Skip((page - 1) * pageInfo.PageSize).Take(pageInfo.PageSize);
            var pageView = new PageView()
            {
                PageInfo = pageInfo,
                TestedLinks = linksOnPage
            };

            return View(pageView);
        }

        [HttpPost]
        public IActionResult Index([FromBody] string link)
        {
            return Content(link);
        }
    }
}
