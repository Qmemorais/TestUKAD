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
        
        public List<UrlModel> GetResults(string url)
        {
            // scan all exist pages on web
            var allUrls = _getLinksFromScanWeb.GetUrlsFromScanPages(url)
                .ToList();
            // find sitemap and if yes: scan
            var linksFromSitemap = _getLinksFromScanSitemap.GetLinksFromSitemapIfExist(url);
            allUrls = AddLinksFromSitemap(allUrls, linksFromSitemap);

            return allUrls;
        }

        public IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(List<UrlModel> htmlToGetTime)
        {
            var values = _getResponseTime.GetLinksWithTime(htmlToGetTime);

            return values;
        }

        private List<UrlModel> AddLinksFromSitemap(List<UrlModel> allUrls, IEnumerable<string> linksFromSitemap)
        {
            var firstLink = allUrls.FirstOrDefault().Link;
            var domainName = _getChanges.GetDomainName(firstLink);

            foreach (var linkFromSitemap in linksFromSitemap)
            {
                var newLinkFromSitemap = _getChanges.GetUrlLikeFromWeb(linkFromSitemap,domainName);
                var indexLinkFromList = allUrls.FindIndex(link => string.Equals(link.Link, newLinkFromSitemap));

                if (indexLinkFromList > -1)
                {
                    allUrls[indexLinkFromList].IsSitemap = true;
                }
                else
                {
                    allUrls.Add(new UrlModel { Link = linkFromSitemap, IsSitemap = true });
                }
            }

            return allUrls;
        }
    }
}
