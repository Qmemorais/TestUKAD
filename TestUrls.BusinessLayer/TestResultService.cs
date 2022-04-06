using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestUrls.EntityFramework.Entities;
using TestUrls.TestResultLogic.Models;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestUrls.TestResultLogic
{
    public class TestResultService
    {
        private readonly IRepository<Test> _testEntities;
        private readonly CrawlerService _crawlerService;

        public TestResultService(
            CrawlerService crawlerService,
            IRepository<Test> testEntities)
        {
            _crawlerService = crawlerService;
            _testEntities = testEntities;
        }

        public int SaveToDatabase(string linkToScan, IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var urlEntity = MappedResultLinks(urlModels, urlResponseModels);
            var testResult = new Test
            {
                Link = linkToScan,
                UrlWithResponseEntities = urlEntity
            };

            _testEntities.AddAsync(testResult);
            _testEntities.SaveChanges();

            return testResult.Id;
        }

        public virtual IEnumerable<TestModel> GetTestedLinks(int pageNumber, int pageSize)
        {
            var testedLinks = _testEntities
                .GetAllAsNoTracking()
                .OrderBy(link => link.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var testResponse = testedLinks.Select(link => new TestModel
            {
                Id = link.Id,
                Link = link.Link,
                CreateAt = link.CreateAt
            });

            return testResponse;
        }

        public int GetTotalCount()
        {
            return _testEntities.GetAllAsNoTracking().Count();
        }

        public virtual IEnumerable<TestResultModel> MappedTestedLinks(string link)
        {
            var testedLinks = GetLinksFromCrawler(link);
            var testedLinkWithResponse = GetLinksFromCrawlerWithResponse(testedLinks);

            var idTestResult = SaveToDatabase(link, testedLinks, testedLinkWithResponse);
            var testResultResponse = GetTestedData(idTestResult);

            return testResultResponse;
        }

        public virtual IEnumerable<TestResultModel> GetTestedData(int id)
        {
            var testedLink = _testEntities
                .Include(link => link.UrlWithResponseEntities)
                .FirstOrDefault(link => link.Id == id);

            var testResultResponse = testedLink.UrlWithResponseEntities
                .Select(link => new TestResultModel
                {
                    Link = link.Link,
                    IsSitemap = link.IsSitemap,
                    IsWeb = link.IsWeb,
                    TimeOfResponse = link.TimeOfResponse
                });

            return testResultResponse
                .OrderBy(link => link.TimeOfResponse)
                .ToList(); ;
        }

        public virtual IEnumerable<UrlModel> GetLinksFromCrawler(string url)
        {
            var linksFromCrawler = _crawlerService.GetResults(url);

            return linksFromCrawler;
        }

        public virtual IEnumerable<UrlModelWithResponse> GetLinksFromCrawlerWithResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var linksWithResponse = _crawlerService.GetUrlsWithTimeResponse(htmlToGetTime);

            return linksWithResponse;
        }

        private ICollection<TestResult> MappedResultLinks(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var resultLinks = new List<TestResult>();

            foreach (var entity in urlModels)
            {
                var timeResponse = urlResponseModels
                    .FirstOrDefault(link => string.Equals(link.Link, entity.Link))
                    .TimeOfResponse;
                resultLinks.Add(
                    new TestResult
                    { 
                        Link = entity.Link, 
                        IsSitemap = entity.IsSitemap, 
                        IsWeb = entity.IsWeb, 
                        TimeOfResponse = timeResponse 
                    });
            }

            return resultLinks;
        }
    }
}
