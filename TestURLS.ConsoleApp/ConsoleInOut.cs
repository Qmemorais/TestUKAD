using System;
using TestURLS.ConsoleApp.Interfaces;

namespace TestURLS.ConsoleApp
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
