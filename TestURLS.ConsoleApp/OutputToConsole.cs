using System.Collections.Generic;
using System.Linq;
using TestURLS.UrlLogic;
using TestURLS.Models;

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

        public virtual void Write(List<UrlWithScanPage> allLinksFromSitemapAndScan)
        {
            if (!allLinksFromSitemapAndScan.Any(link => link.FoundAt == "sitemap"))
            {
                OutputTime(allLinksFromSitemapAndScan.Select(links => links.Link).ToList());
                _consoleInOut.Write($"Urls(html documents) found after crawling a website: {allLinksFromSitemapAndScan.Count}");
            }
            else
            {

                _consoleInOut.Write("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
                OutputURLS(_logic.GetExistLists(allLinksFromSitemapAndScan,"InSitemap"));

                _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
                OutputURLS(_logic.GetExistLists(allLinksFromSitemapAndScan, "InWeb"));

                _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");

                var getLinksFromWeb = allLinksFromSitemapAndScan
                    .Where(link=>link.FoundAt=="web")
                    .Select(links => links.Link)
                    .ToList();
                var getlinksToDistinct = _logic.GetExistLists(allLinksFromSitemapAndScan, "InSitemap");
                OutputTime(getLinksFromWeb.Union(getlinksToDistinct).ToList());

                _consoleInOut.Write($"Urls(html documents) found after crawling a website: {allLinksFromSitemapAndScan.Count(link => link.FoundAt == "web")}");
                _consoleInOut.Write($"Urls found in sitemap: {allLinksFromSitemapAndScan.Count(link => link.FoundAt == "sitemap")}");
            }

        }

        protected virtual void OutputTime(List<string> linkToOutput)
        {
            var urlWithTime = _logic.GetUrlsWithTimeResponse(linkToOutput);

            urlWithTime = urlWithTime.OrderBy(value => value.TimeOfResponse).ToList();

            var lengthURL = linkToOutput.Max(x => x.Length) + 4;

            _consoleInOut.Write(new string('_', lengthURL + 14));

            _consoleInOut.Write($"|{"URL".PadRight(lengthURL, ' ')}|{"Timing (ms)",-12}|");

            _consoleInOut.Write(new string('_', lengthURL + 14));

            for (int i = 0; i < urlWithTime.Count; i++)
            {
                _consoleInOut.Write($"|{((i + 1) + ") " + urlWithTime[i].Link).PadRight(lengthURL, ' ')}|{urlWithTime[i].TimeOfResponse + "ms",-12}|");

                _consoleInOut.Write(new string('_', lengthURL + 14));
            }
        }

        protected virtual void OutputURLS(List<string> linksToOutput)
        {
            var lengthURL = linksToOutput.Max(link => link.Length) + 4;

            _consoleInOut.Write(new string('_', lengthURL + 2));

            _consoleInOut.Write($"|{"URL".PadRight(lengthURL, ' ')}|");

            _consoleInOut.Write(new string('_', lengthURL + 2));

            for (int i = 0; i < linksToOutput.Count; i++)
            {
                _consoleInOut.Write($"|{((i + 1) + ") " + linksToOutput[i]).PadRight(lengthURL, ' ')}|");

                _consoleInOut.Write(new string('_', lengthURL + 2));
            }
        }

    }
}
