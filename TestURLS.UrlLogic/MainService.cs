using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestUrls.EntityFramework.Entities;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class MainService
    {
        private readonly WebService _webService;
        private readonly SitemapService _sitemapService;
        private readonly StringService _stringService;
        private readonly ResponseService _responseService;
        private readonly IRepository<GeneralInfoEntity> _generalInfoEntities;

        public MainService(
            WebService webService,
            SitemapService sitemapService,
            StringService stringService,
            ResponseService responseService,
            IRepository<GeneralInfoEntity> generalInfoEntities)
        {
            _webService = webService;
            _sitemapService = sitemapService;
            _stringService = stringService;
            _responseService = responseService;
            _generalInfoEntities = generalInfoEntities;
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

        public virtual void DownloadToDatabase(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var generalLink = urlModels.FirstOrDefault().Link;
            var urlEntity = new List<UrlEntity>();
            var urlResponseEntity = new List<UrlResponseEntity>();

            foreach (var entity in urlModels)
            {
                urlEntity.Add(
                    new UrlEntity { Link = entity.Link, IsSitemap = entity.IsSitemap, IsWeb = entity.IsWeb });
            }

            foreach (var entity in urlResponseModels)
            {
                urlResponseEntity.Add(
                    new UrlResponseEntity { Link = entity.Link, TimeOfResponse = entity.TimeOfResponse });
            }

            _generalInfoEntities.Add(new GeneralInfoEntity 
                { Link = generalLink, UrlEntities = urlEntity, UrlResponseEntities = urlResponseEntity });
            _generalInfoEntities.SaveChanges();
        }
    }
}
