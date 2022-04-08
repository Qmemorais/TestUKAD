using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestUrl.WebApi.Models;
using TestUrls.TestResultLogic;

namespace TestUrl.WebApi.Controllers
{
    [ApiController]
    [Route("")]
    [Route("Home")]
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
            try
            {
                var totalItems = _testResultService.GetTotalCount();

                if (totalItems == 0)
                {
                    return NotFound();
                }

                var pageInfo = new PageInfo()
                {
                    PageNumber = page,
                    TotalItems = totalItems
                };
                var linksOnPage = _testResultService.GetTestedLinks(page, pageInfo.PageSize).ToList();

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
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
