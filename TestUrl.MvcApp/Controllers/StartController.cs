using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestUrls.BusinessLogic;
using TestUrls.EntityFramework.Entities;

namespace TestUrl.MvcApp.Controllers
{
    [ApiController]
    [Route("Start")]
    public class StartController : Controller
    {
        private readonly BusinessService _businessServer;

        public StartController(BusinessService businessServer)
        {
            _businessServer = businessServer;
        }

        public IActionResult Index()
        {
            IEnumerable<Test> testLinks = _businessServer.GetTestedLinks();

            return View(testLinks);
        }
    }
}
