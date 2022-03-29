using System.Linq;
using TestUrls.BusinessLayer;

namespace TestURLS.ConsoleApp
{
    public class LogicToConsole
    {
        private readonly ConsoleInOut _consoleInOut;
        private readonly BusinesService _businesService;
        private readonly OutputToConsole _outputToConsole;

        public LogicToConsole(
            ConsoleInOut consoleInOut,
            BusinesService businesService,
            OutputToConsole outputToConsole)
        {
            _consoleInOut = consoleInOut;
            _businesService = businesService;
            _outputToConsole = outputToConsole;
        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
            var urlToScan = _consoleInOut.Read();
            var results = _businesService.GetLinksFromCrowler(urlToScan);
            var isSitemapWasFound = results.Any(link => link.IsSitemap);

            if (isSitemapWasFound)
            {
                _outputToConsole.WriteLinksWithoutTime(results);
            }

            var getTime = _businesService.GetLinksFromCrowlerWithResponse(results);
            _outputToConsole.WriteLinksWithTime(getTime);

            _outputToConsole.WriteCountLinks(results);

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
