using System.Collections.Generic;
using TestUrls.TestResultLogic.Models;

namespace TestUrl.MvcApp.Models
{
    public class PageView
    {
        public IEnumerable<TestModel> TestedLinks { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
