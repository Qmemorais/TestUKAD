using System.Collections.Generic;
using System.Linq;
using TestURLS.Models;

namespace TestURLS.UrlLogic
{
    public class MainLogic
    {
        private readonly LogicScanByHTML _scanByHTML = new LogicScanByHTML();
        private readonly LogicScanBySitemap _scanBySitemap = new LogicScanBySitemap();
        private readonly TimeTracker _getTime = new TimeTracker();

        public MainLogic(LogicScanByHTML scanByHTML, LogicScanBySitemap scanBySitemap)
        {
            _scanByHTML = scanByHTML;
            _scanBySitemap = scanBySitemap;
        }

        public MainLogic() { }

        public virtual List<UrlWithScanPage> GetResults(string url)
        {
            var allUrls = new List<UrlWithScanPage>();
            // scan all exist pages on web
            allUrls.AddRange(_scanByHTML.GetUrlsFromScanPages(url));
            // find sitemap and if yes: scan
            allUrls.AddRange(_scanBySitemap.VerifyExistStitemap(url));

            return allUrls;
        }

        public virtual List<string> GetExistLists(List<UrlWithScanPage> allLinksFromAllScan, string whatWeWant)
        {
            var htmlToScan = new List<string>();
            var htmlToMove = new List<string>();
            var listToReturn = new List<string>();

            if (whatWeWant == "InWeb")
            {
                htmlToScan = allLinksFromAllScan
                    .Where(found => found.FoundAt == "web")
                    .Select(links => links.Link)
                    .ToList();
                htmlToMove = allLinksFromAllScan
                    .Where(found => found.FoundAt == "sitemap")
                    .Select(links => links.Link)
                    .ToList();
            }
            else
            {
                htmlToScan = allLinksFromAllScan
                    .Where(found => found.FoundAt == "sitemap")
                    .Select(links => links.Link)
                    .ToList();
                htmlToMove = allLinksFromAllScan
                    .Where(found => found.FoundAt == "web")
                    .Select(links => links.Link)
                    .ToList();

            }

            foreach (string url in htmlToScan)
            {// find any url like this. this foreach better then remove http/https
             // and add after distinct method
                if (!(htmlToMove.Any(web => web.IndexOf(url.Substring("https".Length)) != -1)))
                {
                    listToReturn.Add(url);
                }
            }
            return listToReturn;
        }

        public virtual List<UrlTimeModel> GetUrlsWithTimeResponse(List<string> htmlToGetTime)
        {
            List<UrlTimeModel> values = _getTime.GetLinksWithTime(htmlToGetTime);

            return values;
        }
    }
}
