using Microsoft.AspNetCore.Mvc;
using TestUrls.BusinessLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        private readonly TestResultService _businessService;

        public TestController(TestResultService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("{id}")]
        public IActionResult RunTestLink([FromRoute] int id)
        {
            var testedLinks = _businessService.GetTestedData(id);

            return View("Index", testedLinks);
        }

        [HttpPost]
        public IActionResult RunTestLink([FromForm]string link)
        {
            var mappedTestedLinks = _businessService.MappedTestedLinks(link);

            return View("Index", mappedTestedLinks);
        }
    }
}
