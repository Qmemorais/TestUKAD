using Microsoft.AspNetCore.Mvc;
using TestUrls.BusinessLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        private readonly BusinessService _businessService;

        public TestController(BusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("{id}")]
        public IActionResult RunTestLink([FromRoute] int id)
        {
            var testedLinks = _businessService.GetTestedData(id);

            return View(testedLinks);
        }

        [HttpPost]
        public IActionResult RunTestLink([FromForm]string link)
        {
            var mappedTestedLinks = _businessService.MappedTestedLinks(link);

            return View(mappedTestedLinks);
        }
    }
}
