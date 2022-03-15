using System.Collections.Generic;
using System.Linq;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class MainLogic
    {
        private readonly LogicScanByHtml _scanByHtml = new LogicScanByHtml();
        private readonly LogicScanBySitemap _scanBySitemap = new LogicScanBySitemap();
        private readonly UrlSettings _settings = new UrlSettings();
        private readonly TimeTracker _timeTracker = new TimeTracker();

        public MainLogic(LogicScanByHtml scanByHtml, LogicScanBySitemap scanBySitemap)
        {
            _scanByHtml = scanByHtml;
            _scanBySitemap = scanBySitemap;
        }

        public MainLogic() { }

        public virtual List<UrlModel> GetResults(string url)
        {
            var allUrls = new List<UrlModel>();
            // scan all exist pages on web
            allUrls.AddRange(_scanByHtml.GetUrlsFromScanPages(url));
            // find sitemap and if yes: scan
            var linksFromSitemap = _scanBySitemap.GetLinksFromSitemapIfExist(url);
            allUrls = AddLinksFromSitemap(allUrls, linksFromSitemap);

            return allUrls;
        }
        public virtual IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(List<UrlModel> htmlToGetTime)
        {
            var values = _timeTracker.GetLinksWithTime(htmlToGetTime);

            return values;
        }

        protected virtual List<UrlModel> AddLinksFromSitemap(List<UrlModel> allUrls, IEnumerable<string> linksFromSitemap)
        {
            var firstLink = allUrls.FirstOrDefault().Link;
            var domenName = _settings.GetDomenName(firstLink);

            foreach (var linkFromSitemap in linksFromSitemap)
            {
                var newLinkFromSitemap = _settings.GetUrlLikeFromWeb(linkFromSitemap,domenName);
                var linkIsAlreadyExist = allUrls.Any(link => link.Link.Equals(newLinkFromSitemap));

                if (linkIsAlreadyExist)
                {
                    allUrls
                        .FindAll(link => link.Link.Equals(newLinkFromSitemap))
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
