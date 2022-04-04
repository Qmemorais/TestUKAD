using Microsoft.AspNetCore.Mvc;
using TestUrl.MvcApp.Models;
using TestUrls.TestResultLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        private readonly TestResultService _testResultService;

        public HomeController(TestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet]
        public IActionResult StartCrawler(int page = 1)
        {
            var totalItems = _testResultService.GetTotalCount();
            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                TotalItems = totalItems
            };
            var linksOnPage = _testResultService.GetTestedLinks(page, pageInfo.PageSize);
            var pageView = new PageView()
            {
                PageInfo = pageInfo,
                TestedLinks = linksOnPage
            };

            return View("Index", pageView);
        }
    }
}
