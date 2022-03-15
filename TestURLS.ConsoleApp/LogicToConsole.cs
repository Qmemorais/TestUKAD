using System.Linq;
using TestURLS.UrlLogic;

namespace TestURLS.ConsoleApp
{
    public class LogicToConsole
    {
        private readonly IConsoleInOut _consoleInOut = new ConsoleInOut();
        private readonly MainLogic _logic = new MainLogic();
        private readonly OutputToConsole _outputToConsole = new OutputToConsole();

        public LogicToConsole(
            IConsoleInOut consoleInOut,
            MainLogic logic,
            OutputToConsole outputToConsole)
        {
            _consoleInOut = consoleInOut;
            _logic = logic;
            _outputToConsole = outputToConsole;
        }

        public LogicToConsole()
        {

        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
            var urlToScan = _consoleInOut.Read();
            var results = _logic.GetResults(urlToScan);
            var isSitemapWasFound = results.Any(link => link.IsSitemap == true);

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
