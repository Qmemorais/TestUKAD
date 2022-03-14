using System.Collections.Generic;
using System.Linq;
using TestURLS.UrlLogic;

namespace TestURLS.ConsoleApp
{
    public class OutputToConsole
    {
        private readonly MainLogic _logic = new MainLogic();
        private readonly IConsoleInOut _consoleInOut = new ConsoleInOut();

        public OutputToConsole(IConsoleInOut consoleInOut, MainLogic logic)
        {
            _consoleInOut = consoleInOut;
            _logic = logic;
        }

        public OutputToConsole() { }

        public virtual void Write(List<string> UrlsFromSitemap, List<string> UrlFromWeb)
        {
            if (UrlsFromSitemap.Count == 0)
            {
                OutputTime(UrlFromWeb);
                _consoleInOut.Write($"Urls(html documents) found after crawling a website: {UrlFromWeb.Count}");
            }
            else
            {

                _consoleInOut.Write("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
                OutputURLS(_logic.GetExistLists(UrlsFromSitemap, UrlFromWeb));

                _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
                OutputURLS(_logic.GetExistLists(UrlFromWeb, UrlsFromSitemap));

                _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");
                OutputTime(UrlFromWeb
                    .Union(_logic.GetExistLists(UrlsFromSitemap, UrlFromWeb))
                    .ToList());

                _consoleInOut.Write($"Urls(html documents) found after crawling a website: {UrlFromWeb.Count}");
                _consoleInOut.Write($"Urls found in sitemap: {UrlsFromSitemap.Count}");
            }

        }

        protected virtual void OutputTime(List<string> html)
        {
            var urlWithTime = _logic.GetUrlsWithTimeResponse(html);

            urlWithTime = urlWithTime.OrderBy(value => value.TimeOfResponse).ToList();

            var lengthURL = html.Max(x => x.Length) + 4;

            _consoleInOut.Write(new string('_', lengthURL + 14));

            _consoleInOut.Write(string.Format($"|{"URL".PadRight(lengthURL, ' ')}|{"Timing (ms)",-12}|"));

            _consoleInOut.Write(new string('_', lengthURL + 14));

            for (int i = 0; i < urlWithTime.Count; i++)
            {
                _consoleInOut.Write(string.Format($"|{((i + 1) + ") " + urlWithTime[i].Link).PadRight(lengthURL, ' ')}|{urlWithTime[i].TimeOfResponse + "ms",-12}|"));

                _consoleInOut.Write(new string('_', lengthURL + 14));
            }
        }

        protected virtual void OutputURLS(List<string> html)
        {
            var lengthURL = html.Max(x => x.Length) + 4;

            _consoleInOut.Write(new string('_', lengthURL + 2));

            _consoleInOut.Write(string.Format($"|{"URL".PadRight(lengthURL, ' ')}|"));

            _consoleInOut.Write(new string('_', lengthURL + 2));

            for (int i = 0; i < html.Count; i++)
            {
                _consoleInOut.Write(string.Format($"|{((i + 1) + ") " + html[i]).PadRight(lengthURL, ' ')}|"));

                _consoleInOut.Write(new string('_', lengthURL + 2));
            }
        }

    }
}
