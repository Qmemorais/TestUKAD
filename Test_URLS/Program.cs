using System;

namespace Test_URLS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter URL: ");
            string url = Console.ReadLine();
            FindURL getData = new FindURL();
            getData.GetContent(url);
        }
    }
}
