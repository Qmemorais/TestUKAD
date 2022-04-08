﻿using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TestUrls.TestResultLogic;

namespace TestUrl.WebApi.Controllers
{
    [ApiController]
    [Route("Test")]
    public class TestController : ControllerBase
    {
        private readonly TestResultService _testResultService;

        public TestController(TestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        [HttpGet("{id}")]
        public IActionResult GetTestLink([FromRoute] int id)
        {
            try
            {
                var testedLinks = _testResultService.GetTestedData(id);

                if (testedLinks == null)
                {
                    return NotFound();
                }

                return Ok(testedLinks);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public IActionResult RunTestLink([FromForm] string link)
        {
            try
            {
                if (string.IsNullOrEmpty(link))
                {
                    return BadRequest();
                }

                var idTest = _testResultService.TestLink(link);
                var testedLinks = _testResultService.GetTestedData(idTest);

                return Ok(testedLinks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}