using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestUrls.BusinessLogic.BusinessModels;
using TestUrls.EntityFramework.Entities;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestUrls.BusinessLogic
{
    public class BusinessService
    {
        private readonly IRepository<Test> _testEntities;
        private readonly MainService _mainService;

        public BusinessService(
            MainService mainService,
            IRepository<Test> testEntities)
        {
            _mainService = mainService;
            _testEntities = testEntities;
        }

        public virtual void SaveToDatabase(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var generalLink = urlModels.FirstOrDefault().Link;
            var urlEntity = new List<TestResult>();

            foreach (var entity in urlModels)
            {
                urlEntity.Add(
                    new TestResult { Link = entity.Link, IsSitemap = entity.IsSitemap, IsWeb = entity.IsWeb });
            }

            foreach (var entity in urlResponseModels)
            {
                urlEntity
                    .FindAll(url => url.Link == entity.Link)
                    .ForEach(link => link.TimeOfResponse = entity.TimeOfResponse);
            }

            _testEntities.Add(new Test
                { Link = generalLink, UrlWithResponseEntities = urlEntity });
            _testEntities.SaveChanges();
        }

        public virtual IEnumerable<TestDto> GetTestedLinks()
        {
            var testResponse = new List<TestDto>();
            var testedLinks = _testEntities.GetAll();

            foreach(var link in testedLinks)
            {
                testResponse.Add(new TestDto 
                    { Id = link.Id, Link = link.Link, CreateAt = link.CreateAt });
            }

            return testResponse;
        }

        public virtual IEnumerable<TestResultDto> GetTestedData(int id)
        {
            var testResultResponse = new List<TestResultDto>();
            var urlModels = _testEntities.GetById(id).UrlWithResponseEntities;

            foreach(var link in urlModels)
            {
                testResultResponse.Add(new TestResultDto 
                    {Link= link.Link, IsSitemap= link.IsSitemap, IsWeb= link.IsWeb, TimeOfResponse= link.TimeOfResponse });
            }

            return testResultResponse;
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
