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
        private readonly BusinessService _businessService;

        public HomeController(BusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet]
        public IActionResult RunCrawler(int page)
        {
            var testedLinks = _businessService.GetTestedLinks(page).ToList();
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
        public IActionResult RunCrawler([FromBody] string link)
        {
            return Content(link);
        }
    }
}
