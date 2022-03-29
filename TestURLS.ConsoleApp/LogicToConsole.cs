using System.Linq;
using TestURLS.UrlLogic;

namespace TestURLS.ConsoleApp
{
    public class LogicToConsole
    {
        private readonly ConsoleInOut _consoleInOut;
        private readonly MainService _logic;
        private readonly OutputToConsole _outputToConsole;

        public LogicToConsole(
            ConsoleInOut consoleInOut,
            MainService logic,
            OutputToConsole outputToConsole)
        {
            _consoleInOut = consoleInOut;
            _logic = logic;
            _outputToConsole = outputToConsole;
        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
            var urlToScan = _consoleInOut.Read();
            var results = _logic.GetResults(urlToScan);
            var isSitemapWasFound = results.Any(link => link.IsSitemap);

            if (isSitemapWasFound)
            {
                _outputToConsole.WriteLinksWithoutTime(results);
            }

            var getTime = _logic.GetUrlsWithTimeResponse(results);
            _outputToConsole.WriteLinksWithTime(getTime);

            _outputToConsole.WriteCountLinks(results);

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
