using System.Linq;
using TestUrls.BusinessLogic;

namespace TestURLS.ConsoleApp
{
    public class LogicToConsole
    {
        private readonly ConsoleInOut _consoleInOut;
        private readonly BusinessService _businessService;
        private readonly OutputToConsole _outputToConsole;

        public LogicToConsole(
            ConsoleInOut consoleInOut,
            BusinessService businessService,
            OutputToConsole outputToConsole)
        {
            _consoleInOut = consoleInOut;
            _businessService = businessService;
            _outputToConsole = outputToConsole;
        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
            var urlToScan = _consoleInOut.Read();
            var results = _businessService.GetLinksFromCrawler(urlToScan);
            var isSitemapWasFound = results.Any(link => link.IsSitemap);

            if (isSitemapWasFound)
            {
                _outputToConsole.WriteLinksWithoutTime(results);
            }

            var getTime = _businessService.GetLinksFromCrawlerWithResponse(results);
            _outputToConsole.WriteLinksWithTime(getTime);

            _outputToConsole.WriteCountLinks(results);

            _businessService.SaveToDatabase(results, getTime);

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
