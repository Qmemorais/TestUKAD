using System.Collections.Generic;
using TestUrls.TestResultLogic.BusinessModels;

namespace TestUrl.MvcApp.Models
{
    public class PageView
    {
        public IEnumerable<TestDto> TestedLinks { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
