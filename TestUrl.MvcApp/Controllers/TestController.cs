using Microsoft.AspNetCore.Mvc;

namespace TestUrl.MvcApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
