using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp
{
    public class OutputToConsole
    {
        private readonly ConsoleInOut _consoleInOut;

        public OutputToConsole(ConsoleInOut consoleInOut)
        {
            _consoleInOut = consoleInOut;
        }

        public virtual void WriteLinksWithoutTime(IEnumerable<UrlModel> allLinksFromSitemapAndScan)
        {
            _consoleInOut.Write("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            OutputUrls(allLinksFromSitemapAndScan.Where(linkFromWeb => linkFromWeb.IsWeb == false).ToList());

            _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            OutputUrls(allLinksFromSitemapAndScan.Where(linkFromWeb => linkFromWeb.IsSitemap == false).ToList());
        }

        public virtual void WriteLinksWithTime(IEnumerable<UrlModelWithResponse> linksWithResponseTime)
        {
            _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");
            OutputTime(linksWithResponseTime);
        }

        public virtual void WriteCountLinks(IEnumerable<UrlModel> allLinksFromSitemapAndScan)
        {
            _consoleInOut.Write($"Urls(html documents) found after crawling a website: {allLinksFromSitemapAndScan.Count(link => link.IsWeb == true)}");
            _consoleInOut.Write($"Urls found in sitemap: {allLinksFromSitemapAndScan.Count(link => link.IsSitemap == true)}");
        }

        private void OutputTime(IEnumerable<UrlModelWithResponse> linksToOutput)
        {
            var stringToWrite = new StringBuilder();

            var lengthURL = linksToOutput.Max(link => link.Link.Length) + 4;
            var stringWithSymbols = new string('_', lengthURL + 14);

            stringToWrite.AppendLine(stringWithSymbols);

            stringToWrite.Append($"|{"URL".PadRight(lengthURL, ' ')}")
                .AppendLine($"|{ "Timing (ms)",-12}|");

            stringToWrite.AppendLine(stringWithSymbols);

            var numberOfLink = 1;

            foreach(var modelWitTime in linksToOutput)
            {
                var linkToWrite = $"{numberOfLink}) {modelWitTime.Link}";
                var timeToWrite = $"{modelWitTime.TimeOfResponse}ms";
                stringToWrite
                    .Append($"|{linkToWrite.PadRight(lengthURL, ' ')}")
                    .AppendLine($"|{timeToWrite,-12}|");

                stringToWrite.AppendLine(stringWithSymbols);
                numberOfLink++;
            }

            _consoleInOut.Write(stringToWrite.ToString());
        }

        private void OutputUrls(IEnumerable<UrlModel> linksToOutput)
        {
            var stringToWrite = new StringBuilder();
            var lengthURL = linksToOutput.Max(link => link.Link.Length) + 4;
            var stringWithSymbols = new string('_', lengthURL + 2);

            stringToWrite.AppendLine(stringWithSymbols);

            stringToWrite.AppendLine($"|{"URL".PadRight(lengthURL, ' ')}|");

            stringToWrite.AppendLine(stringWithSymbols);

            var numberOfLink = 1;

            foreach (var modelWitTime in linksToOutput)
            {
                var linkToWrite = $"{numberOfLink}) {modelWitTime.Link}";
                stringToWrite.AppendLine($"|{linkToWrite.PadRight(lengthURL, ' ')}|");
                stringToWrite.AppendLine(stringWithSymbols);

                numberOfLink++;
            }

            _consoleInOut.Write(stringToWrite.ToString());
        }

    }
}
