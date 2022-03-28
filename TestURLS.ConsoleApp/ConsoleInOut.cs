using System;

namespace TestURLS.ConsoleApp
{
    public class ConsoleInOut
    {
        public virtual void Write(string s)
        {
            Console.WriteLine(s);
        }

        public virtual string Read()
        {
            return Console.ReadLine();
        }
    }
}
