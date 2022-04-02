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
                var timeResponse = urlResponseModels
                    .First(link => string.Equals(link.Link, entity.Link))
                    .TimeOfResponse;
                urlEntity.Add(
                    new TestResult
                    { Link = entity.Link, IsSitemap = entity.IsSitemap, IsWeb = entity.IsWeb, TimeOfResponse = timeResponse });
            }

            _testEntities.Add(new Test
                { Link = generalLink, UrlWithResponseEntities = urlEntity });
            _testEntities.SaveChanges();
        }

        public virtual IEnumerable<TestDto> GetTestedLinks(int pageNumber, int pageSize)
        {
            var testResponse = new List<TestDto>();
            var testedLinks = _testEntities
                .GetAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            foreach (var link in testedLinks)
            {
                testResponse.Add(new TestDto 
                    { Id = link.Id, Link = link.Link, CreateAt = link.CreateAt });
            }

            return testResponse;
        }

        public virtual IEnumerable<TestResultDto> MappedTestedLinks(string link)
        {
            var testedLinks = GetLinksFromCrawler(link);
            var testedLinkWithResponse = GetLinksFromCrawlerWithResponse(testedLinks);

            SaveToDatabase(testedLinks, testedLinkWithResponse);
            var testResultResponse = new List<TestResultDto>();

            foreach (var entity in testedLinks)
            {
                var timeResponse = testedLinkWithResponse
                    .First(link => string.Equals(link.Link, entity.Link))
                    .TimeOfResponse;
                testResultResponse.Add(
                    new TestResultDto
                    { Link = entity.Link, IsSitemap = entity.IsSitemap, IsWeb = entity.IsWeb, TimeOfResponse = timeResponse });
            }

            testResultResponse = testResultResponse
                .OrderBy(link => link.TimeOfResponse)
                .ToList();

            return testResultResponse;
        }

        public virtual IEnumerable<TestResultDto> GetTestedData(int id)
        {
            var testResultResponse = new List<TestResultDto>();
            var testedLink = _testEntities
                .Include(link => link.UrlWithResponseEntities)
                .FirstOrDefault(link => link.Id == id);

            foreach(var link in testedLink.UrlWithResponseEntities)
            {
                testResultResponse.Add(new TestResultDto 
                    {Link= link.Link, IsSitemap= link.IsSitemap, IsWeb= link.IsWeb, TimeOfResponse= link.TimeOfResponse });
            }

            return testResultResponse
                .OrderBy(link => link.TimeOfResponse)
                .ToList(); ;
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
