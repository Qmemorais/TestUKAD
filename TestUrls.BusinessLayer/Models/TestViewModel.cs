using System.Collections.Generic;

namespace TestUrls.TestResultLogic.Models
{
    public class TestViewModel
    {
        public string TestedLink { get; set; }
        public IEnumerable<TestResultModel> ListOfScan { get; set; }
    }
}
