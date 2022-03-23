using System.Linq;
using TestURLS.ConsoleApp.Interfaces;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.ConsoleApp
{
    public class LogicToConsole
    {
        private readonly IConsoleInOut _consoleInOut;
        private readonly IMainLogic _logic;
        private readonly IOutputToConsole _outputToConsole;

        public LogicToConsole(
            IConsoleInOut consoleInOut,
            IMainLogic logic,
            IOutputToConsole outputToConsole)
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
            _outputToConsole.WriteLinksWithTime(getTime.ToList());

            _outputToConsole.WriteCountLinks(results);

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
