using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestUrls.EntityFramework.Entities;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestUrls.BusinessLayer
{
    public class BusinesService
    {
        private IRepository<GeneralInfoEntity> _generalInfoEntities;
        private MainService _mainService;

        public BusinesService(
            MainService mainService,
            IRepository<GeneralInfoEntity> generalInfoEntities)
        {
            _mainService = mainService;
            _generalInfoEntities = generalInfoEntities;
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

            _generalInfoEntities.Add(new GeneralInfoEntity
                { Link = generalLink, UrlWithResponseEntities = urlEntity });
            _generalInfoEntities.SaveChanges();
        }

        public virtual IEnumerable<GeneralInfoEntity> GetAllGeneralLinks()
        {
            var urlWithResponse = _generalInfoEntities.GetAll();

            return urlWithResponse;
        }

        public virtual IEnumerable<UrlModel> GetLinksFromCrowler(string url)
        {
            var linksFromCrowler = _mainService.GetResults(url);

            return linksFromCrowler;
        }

        public virtual IEnumerable<UrlModelWithResponse> GetLinksFromCrowlerWithResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var linksWithResponse = _mainService.GetUrlsWithTimeResponse(htmlToGetTime);

            return linksWithResponse;
        }
    }
}
