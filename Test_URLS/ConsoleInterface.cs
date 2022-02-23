using Test_URLS.urlLogic;

namespace Test_URLS.ConsoleApp
{
    class ConsoleInterface
    {
        private IConsoleInOut _consoleInOut = new ConsoleInOut();
        private MainLogic _logic = new MainLogic();

        public ConsoleInterface(IConsoleInOut consoleInOut)
        {
            _consoleInOut = consoleInOut;
        }

        public ConsoleInterface()
        {

        }

        public void Start()
        {
            _consoleInOut.Write("Enter URL: ");
                string urlToScan = _consoleInOut.Read();

            var resultList = _logic.GetResults(urlToScan);
            foreach(string s in resultList)
            {
                _consoleInOut.Write(s);
            }

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
