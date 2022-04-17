using System;

namespace TestURLS.ConsoleApp
{
    public class ConsoleInOut
    {
        public virtual void Write(string line)
        {
            Console.WriteLine(line);
        }

        public virtual string Read()
        {
            return Console.ReadLine();
        }
    }
}
