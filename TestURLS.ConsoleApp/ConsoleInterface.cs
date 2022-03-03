using System;
using TestURLS.UrlLogic;

namespace TestURLS.ConsoleApp
{
    public class ConsoleInterface
    {
        private readonly IConsoleInOut _consoleInOut = new ConsoleInOut();
        private readonly MainLogic _logic = new MainLogic();

        public ConsoleInterface(IConsoleInOut consoleInOut, MainLogic logic)
        {
            _consoleInOut = consoleInOut;
            _logic = logic;
        }

        public ConsoleInterface()
        {

        }

        public void Start()
        {
            try
            {
                _consoleInOut.Write("Enter URL: ");
                string urlToScan = _consoleInOut.Read();

                var resultList = _logic.GetResults(urlToScan);

                foreach (string s in resultList)
                {
                    _consoleInOut.Write(s);
                }
            }
            catch(Exception ex)
            {
                _consoleInOut.Write(ex.Message);
            }

            _consoleInOut.Write("Press <Enter>");
            _consoleInOut.Read();
        }
    }
}
