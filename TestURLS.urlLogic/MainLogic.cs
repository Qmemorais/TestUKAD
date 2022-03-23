using System.Collections.Generic;
using System.Linq;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class MainLogic : IMainLogic
    {
        private readonly ILogicToGetLinksFromScanWeb _getLinksFromScanWeb;
        private readonly ILogicToGetLinksFromSitemap _getLinksFromScanSitemap;
        private readonly ChangesAboveLink _getChanges;
        private readonly ResponseTimeOfUrl _getResponseTime;

        public MainLogic(
            ILogicToGetLinksFromScanWeb scanByHtml, 
            ILogicToGetLinksFromSitemap scanBySitemap,
            ChangesAboveLink settings,
            ResponseTimeOfUrl timeTracker)
        {
            _getLinksFromScanWeb = scanByHtml;
            _getLinksFromScanSitemap = scanBySitemap;
            _getChanges = settings;
            _getResponseTime = timeTracker;
        }
        
        public IEnumerable<UrlModel> GetResults(string url)
        {
            // scan all exist pages on web
            var linksFromScanPages = _getLinksFromScanWeb.GetUrlsFromScanPages(url);
            // find sitemap and if yes: scan
            var linksFromScanSitemap = _getLinksFromScanSitemap.GetLinksFromSitemapIfExist(url);
            var allUrls = GetLinksWithUrlModel(linksFromScanPages.ToList(), linksFromScanSitemap);

            return allUrls;
        }

        public IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var values = _getResponseTime.GetLinksWithTime(htmlToGetTime);

            return values;
        }

        private List<UrlModel> GetLinksWithUrlModel(
            List<string> linksFromScanPages,
            IEnumerable<string> linksFromScanSitemap)
        {
            var allLinks = new List<UrlModel>();
            var firstLink = linksFromScanPages.FirstOrDefault();
            var domainName = _getChanges.GetDomainName(firstLink);

            foreach (var linkFromList in linksFromScanSitemap)
            {
                var newLinkFromSitemap = _getChanges.GetUrlLikeFromWeb(linkFromList, domainName);
                var indexLinkFromList = linksFromScanPages.FindIndex(link => string.Equals(link, newLinkFromSitemap));

                if (indexLinkFromList > -1)
                {
                    allLinks.Add(new UrlModel { Link = newLinkFromSitemap, IsSitemap = true, IsWeb = true });
                    linksFromScanPages.RemoveAt(indexLinkFromList);
                }
                else
                {
                    allLinks.Add(new UrlModel { Link = newLinkFromSitemap, IsSitemap = true });
                }
            }

            foreach(var linkFromPage in linksFromScanPages)
            {
                allLinks.Add(new UrlModel { Link = linkFromPage, IsWeb = true });
            }

            return allLinks;
        }
    }
}
