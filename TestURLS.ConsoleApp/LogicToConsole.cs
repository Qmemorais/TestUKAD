using System.Linq;
using TestUrls.TestResultLogic;

namespace TestURLS.ConsoleApp
{
    public class LogicToConsole
    {
        private readonly ConsoleInOut _consoleInOut;
        private readonly TestResultService _testResultService;
        private readonly OutputToConsole _outputToConsole;

        public LogicToConsole(
            ConsoleInOut consoleInOut,
            TestResultService testResultService,
            OutputToConsole outputToConsole)
        {
            _consoleInOut = consoleInOut;
            _testResultService = testResultService;
            _outputToConsole = outputToConsole;
        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
            var urlToScan = _consoleInOut.Read();
            var results = _testResultService.GetLinksFromCrawler(urlToScan);
            var isSitemapWasFound = results.Any(link => link.IsSitemap);

            if (isSitemapWasFound)
            {
                _outputToConsole.WriteLinksWithoutTime(results);
            }

            var getTime = _testResultService.GetLinksFromCrawlerWithResponse(results);
            _outputToConsole.WriteLinksWithTime(getTime);

            _outputToConsole.WriteCountLinks(results);

            _testResultService.SaveToDatabase(urlToScan, results, getTime);

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
