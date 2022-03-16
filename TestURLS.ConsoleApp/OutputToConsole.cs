using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestURLS.ConsoleApp.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp
{
    public class OutputToConsole : IOutputToConsole
    {
        private readonly IConsoleInOut _consoleInOut;

        public OutputToConsole(IConsoleInOut consoleInOut)
        {
            _consoleInOut = consoleInOut;
        }

        public virtual void WriteLinksWithoutTime(List<UrlModel> allLinksFromSitemapAndScan)
        {
            _consoleInOut.Write("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            OutputUrls(allLinksFromSitemapAndScan.Where(linkFromWeb => linkFromWeb.IsWeb == false).ToList());

            _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            OutputUrls(allLinksFromSitemapAndScan.Where(linkFromWeb => linkFromWeb.IsSitemap == false).ToList());
        }

        public virtual void WriteLinksWithTime(List<UrlModelWithResponse> linksWithResponseTime)
        {
            _consoleInOut.Write("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");
            OutputTime(linksWithResponseTime);
        }

        public virtual void WriteCountLinks(List<UrlModel> allLinksFromSitemapAndScan)
        {
            _consoleInOut.Write($"Urls(html documents) found after crawling a website: {allLinksFromSitemapAndScan.Count(link => link.IsWeb == true)}");
            _consoleInOut.Write($"Urls found in sitemap: {allLinksFromSitemapAndScan.Count(link => link.IsSitemap == true)}");
        }

        protected virtual void OutputTime(List<UrlModelWithResponse> linksToOutput)
        {
            var stringToWrite = new StringBuilder();
            linksToOutput = linksToOutput.OrderBy(value => value.TimeOfResponse).ToList();

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

        protected virtual void OutputUrls(List<UrlModel> linksToOutput)
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
