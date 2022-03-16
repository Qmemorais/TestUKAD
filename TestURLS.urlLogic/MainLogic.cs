using System.Collections.Generic;
using System.Linq;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class MainLogic : IMainLogic
    {
        private readonly ILogicScanByHtml _scanByHtml;
        private readonly ILogicScanBySitemap _scanBySitemap;
        private readonly IUrlSettings _settings;
        private readonly ITimeTracker _timeTracker;

        public MainLogic(
            ILogicScanByHtml scanByHtml, 
            ILogicScanBySitemap scanBySitemap,
            IUrlSettings settings,
            ITimeTracker timeTracker)
        {
            _scanByHtml = scanByHtml;
            _scanBySitemap = scanBySitemap;
            _settings = settings;
            _timeTracker = timeTracker;
        }
        public List<UrlModel> GetResults(string url)
        {
            var allUrls = new List<UrlModel>();
            // scan all exist pages on web
            allUrls.AddRange(_scanByHtml.GetUrlsFromScanPages(url));
            // find sitemap and if yes: scan
            var linksFromSitemap = _scanBySitemap.GetLinksFromSitemapIfExist(url);
            allUrls = AddLinksFromSitemap(allUrls, linksFromSitemap);

            return allUrls;
        }

        public IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(List<UrlModel> htmlToGetTime)
        {
            var values = _timeTracker.GetLinksWithTime(htmlToGetTime);

            return values;
        }

        protected virtual List<UrlModel> AddLinksFromSitemap(List<UrlModel> allUrls, IEnumerable<string> linksFromSitemap)
        {
            var firstLink = allUrls.FirstOrDefault().Link;
            var domenName = _settings.GetDomainName(firstLink);

            foreach (var linkFromSitemap in linksFromSitemap)
            {
                var newLinkFromSitemap = _settings.GetUrlLikeFromWeb(linkFromSitemap,domenName);
                var linkIsAlreadyExist = allUrls.Any(link => string.Equals(link.Link, newLinkFromSitemap));

                if (linkIsAlreadyExist)
                {
                    allUrls
                        .FindAll(link => string.Equals(link.Link, newLinkFromSitemap))
                        .ForEach(link => link.IsSitemap = true);
                }
                else
                {
                    allUrls.Add(new UrlModel { Link = linkFromSitemap, IsSitemap = true, IsWeb = false });
                }
            }

            return allUrls;
        }
    }
}
