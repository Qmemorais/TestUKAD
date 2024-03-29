﻿using Microsoft.AspNetCore.Mvc;
using TestUrls.TestResultLogic;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : Controller
    {
        private readonly TestResultService _testResultService;

        public TestController(TestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet("{id}")]
        public IActionResult GetTestLink([FromRoute] int id)
        {
            var testedLinks = _testResultService.GetTestedData(id);

            return View("Index", testedLinks);
        }

        [HttpPost]
        public IActionResult RunTestLink([FromForm]string link)
        {
            var idTest = _testResultService.TestLink(link);
            var testedLinks = _testResultService.GetTestedData(idTest);

            return View("Index", testedLinks);
        }
    }
}
