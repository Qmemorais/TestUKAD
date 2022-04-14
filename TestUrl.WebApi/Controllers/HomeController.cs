using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestUrl.WebApi.Models;
using TestUrls.TestResultLogic;

namespace TestUrl.WebApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly TestResultService _testResultService;

        public HomeController(TestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet("{page}")]
        public IActionResult GetTests(int page = 1)
        {
            var totalItems = _testResultService.GetTotalCount();
            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                TotalItems = totalItems
            };
            var linksOnPage = _testResultService.GetTestedLinks(page, pageInfo.PageSize).ToList();
            var pageView = new PageView()
            {
                PageInfo = pageInfo,
                TestedLinks = linksOnPage
            };

            return Ok(pageView);
        }
    }
}
