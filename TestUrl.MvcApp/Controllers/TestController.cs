using Microsoft.AspNetCore.Mvc;
using TestUrls.BusinessLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        private readonly BusinessService _businessServer;

        public TestController(BusinessService businessServer)
        {
            _businessServer = businessServer;
        }

        [HttpGet("{id}")]
        public IActionResult Index([FromRoute] int id)
        {
            var testedLinks = _businessServer.GetTestedData(id);

            return View(testedLinks);
        }

        [HttpPost]
        public IActionResult Index([FromForm]string link)
        {
            var testedLinks = _businessServer.GetLinksFromCrawler(link);
            var testedLinkWithResponse = _businessServer.GetLinksFromCrawlerWithResponse(testedLinks);

            _businessServer.SaveToDatabase(testedLinks, testedLinkWithResponse);

            var mappedTestedLinks = _businessServer.MappedTestedLinks(testedLinks, testedLinkWithResponse);

            return View(mappedTestedLinks);
        }
    }
}
