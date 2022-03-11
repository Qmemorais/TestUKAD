using TestURLS.UrlLogic;

namespace TestURLS.ConsoleApp
{
    public class ConsoleInterface
    {
        private readonly IConsoleInOut _consoleInOut = new ConsoleInOut();
        private readonly MainLogic _logic = new MainLogic();
        private readonly OutputToConsole _outputToConsole = new OutputToConsole();

        public ConsoleInterface(IConsoleInOut consoleInOut, MainLogic logic,
            OutputToConsole outputToConsole)
        {
            _consoleInOut = consoleInOut;
            _logic = logic;
            _outputToConsole = outputToConsole;
        }

        public ConsoleInterface()
        {

        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
            string urlToScan = _consoleInOut.Read();

            _logic.GetResults(urlToScan);

            _outputToConsole.Write(_logic.HtmlGetUrlFromSitemap,
                _logic.HtmlGetUrlFromWeb);

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
