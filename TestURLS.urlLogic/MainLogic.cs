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
            var allUrls = GetLinksWithUrlModel(linksFromScanPages, linksFromScanSitemap);

            return allUrls;
        }

        public IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var values = _getResponseTime.GetLinksWithTime(htmlToGetTime);

            return values;
        }

        private List<UrlModel> GetLinksWithUrlModel(
            IEnumerable<string> linksFromScanPages,
            IEnumerable<string> linksFromScanSitemap)
        {
            var urlModelFromPages = MakeUrlModelFromScanPages(linksFromScanPages);
            var allLinks = MakeUrlModelFromScanSitemap(urlModelFromPages, linksFromScanSitemap);

            return allLinks;
        }

        private List<UrlModel> MakeUrlModelFromScanPages(IEnumerable<string> linksFromScanPages)
        {
            List<UrlModel> urlModelFromPages = new List<UrlModel>();

            foreach(var link in linksFromScanPages)
            {
                urlModelFromPages.Add(new UrlModel { Link = link, IsWeb = true });
            }

            return urlModelFromPages;
        }

        private List<UrlModel> MakeUrlModelFromScanSitemap(
            List<UrlModel> urlModelFromPages, 
            IEnumerable<string> linksFromScanSitemap)
        {
            List<UrlModel> urlModelFromSitemap = new List<UrlModel>();
            var firstLink = urlModelFromPages.FirstOrDefault().Link;
            var domainName = _getChanges.GetDomainName(firstLink);

            foreach (var linkFromList in linksFromScanSitemap)
            {
                var newLinkFromSitemap = _getChanges.GetUrlLikeFromWeb(linkFromList, domainName);
                var indexLinkFromList = urlModelFromPages.FindIndex(link => string.Equals(link.Link, newLinkFromSitemap));

                if (indexLinkFromList > -1)
                {
                    urlModelFromPages[indexLinkFromList].IsSitemap = true;
                }
                else
                {
                    urlModelFromPages.Add(new UrlModel { Link = linkFromList, IsSitemap = true });
                }
            }

            return urlModelFromPages;
        }
    }
}
