using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class CrawlerService
    {
        private readonly WebService _webService;
        private readonly SitemapService _sitemapService;
        private readonly StringService _stringService;
        private readonly ResponseService _responseService;

        public CrawlerService(
            WebService webService,
            SitemapService sitemapService,
            StringService stringService,
            ResponseService responseService)
        {
            _webService = webService;
            _sitemapService = sitemapService;
            _stringService = stringService;
            _responseService = responseService;
        }

        public virtual IEnumerable<UrlModel> GetResults(string url)
        {
            // scan all exist pages on web
            var linksFromScanPages = _webService.GetUrlsFromScanPages(url);
            // find sitemap and if yes: scan
            var linksFromScanSitemap = _sitemapService.GetLinksFromSitemapIfExist(url);

            var allUrls = GetLinksWithUrlModel(linksFromScanPages.ToList(), linksFromScanSitemap);

            return allUrls;
        }

        public virtual IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var values = _responseService
                .GetLinksWithTime(htmlToGetTime)
                .OrderBy(value => value.TimeOfResponse)
                .ToList();

            return values;
        }

        private IEnumerable<UrlModel> GetLinksWithUrlModel(List<string> linksFromScanPages, IEnumerable<string> linksFromScanSitemap)
        {
            var allLinks = new List<UrlModel>();
            var firstLink = linksFromScanPages.FirstOrDefault();
            var domainName = _stringService.GetDomainName(firstLink);

            foreach (var linkFromPage in linksFromScanPages)
            {
                allLinks.Add(new UrlModel { Link = linkFromPage, IsWeb = true });
            }

            foreach (var linkFromList in linksFromScanSitemap)
            {
                var newLinkFromSitemap = _stringService.GetUrlLikeFromWeb(linkFromList, domainName);
                var indexLinkFromList = linksFromScanPages.FindIndex(link => string.Equals(link, newLinkFromSitemap));

                if (indexLinkFromList > -1)
                {
                    allLinks
                        .FindAll(link => link.Link == newLinkFromSitemap)
                        .ForEach(link => link.IsSitemap = true);
                }
                else
                {
                    allLinks.Add(new UrlModel { Link = newLinkFromSitemap, IsSitemap = true });
                }
            }

            return allLinks;
        }
    }
}
