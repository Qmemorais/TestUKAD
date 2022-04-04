using Microsoft.AspNetCore.Mvc;
using TestUrl.MvcApp.Models;
using TestUrls.BusinessLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly TestResultService _businessService;

        public HomeController(TestResultService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet]
        public IActionResult RunCrawler(int page = 1)
        {
            var totalItems = _businessService.GetTotalCount();
            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                TotalItems = totalItems
            };
            var linksOnPage = _businessService.GetTestedLinks(page, pageInfo.PageSize);
            var pageView = new PageView()
            {
                PageInfo = pageInfo,
                TestedLinks = linksOnPage
            };

            return View("Index", pageView);
        }

        [HttpPost]
        public IActionResult RunCrawler([FromBody] string link)
        {
            return Content("Index", link);
        }
    }
}
