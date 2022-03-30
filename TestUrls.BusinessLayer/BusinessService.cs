using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestUrls.EntityFramework.Entities;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestUrls.BusinessLogic
{
    public class BusinessService
    {
        private readonly IRepository<SiteTestEntity> _testEntities;
        private readonly MainService _mainService;

        public BusinessService(
            MainService mainService,
            IRepository<SiteTestEntity> testEntities)
        {
            _mainService = mainService;
            _testEntities = testEntities;
        }

        public virtual void DownloadToDatabase(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var generalLink = urlModels.FirstOrDefault().Link;
            var urlEntity = new List<UrlWithResponse>();

            foreach (var entity in urlModels)
            {
                urlEntity.Add(
                    new UrlWithResponse { Link = entity.Link, IsSitemap = entity.IsSitemap, IsWeb = entity.IsWeb });
            }

            foreach (var entity in urlResponseModels)
            {
                urlEntity
                    .FindAll(url => url.Link == entity.Link)
                    .ForEach(link => link.TimeOfResponse = entity.TimeOfResponse);
            }

            _testEntities.Add(new SiteTestEntity
                { Link = generalLink, UrlWithResponseEntities = urlEntity });
            _testEntities.SaveChanges();
        }

        public virtual IEnumerable<UrlModel> GetLinksFromCrawler(string url)
        {
            var linksFromCrowler = _mainService.GetResults(url);

            return linksFromCrowler;
        }

        public virtual IEnumerable<UrlModelWithResponse> GetLinksFromCrawlerWithResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var linksWithResponse = _mainService.GetUrlsWithTimeResponse(htmlToGetTime);

            return linksWithResponse;
        }
    }
}
