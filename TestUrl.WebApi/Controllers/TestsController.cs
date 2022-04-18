using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestUrl.WebApi.Models;
using TestUrls.TestResultLogic;

namespace TestUrl.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly TestResultService _testResultService;

        public TestsController(TestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet]
        public IActionResult GetTests()
        {
            var totalItems = _testResultService.GetTotalCount();

            if (totalItems == 0)
            {
                return NotFound();
            }

            var pageInfo = new PageInfo()
            {
                TotalItems = totalItems
            };
            var linksOnPage = _testResultService.GetTestedLinks();

            if (!linksOnPage.Any())
            {
                return NotFound();
            }

            var pageView = new PageView()
            {
                PageInfo = pageInfo,
                TestedLinks = linksOnPage
            };

            return Ok(pageView);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestLink([FromRoute] int id)
        {
            var testedLinks = _testResultService.GetTestedData(id);

            if (testedLinks == null)
            {
                return NotFound();
            }

            return Ok(testedLinks);
        }

        [HttpPost]
        public IActionResult RunTestLink([FromForm] string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return BadRequest();
            }

            var idTest = _testResultService.TestLink(link);
            var testedLinks = _testResultService.GetTestedData(idTest);

            return Ok(testedLinks);
        }
    }
}
