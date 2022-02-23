using System;

namespace Test_URLS.ConsoleApp
{
    class ConsoleInOut:IConsoleInOut
    {
        public void Write(string s)
        {
            Console.WriteLine(s);
        }

        public string Read()
        {
            return Console.ReadLine();
        }
    }
}
