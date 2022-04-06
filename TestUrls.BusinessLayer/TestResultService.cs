using System.Collections.Generic;
using System.Data;
using System.Linq;
using TestUrls.EntityFramework.Entities;
using TestUrls.TestResultLogic.BusinessModels;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestUrls.TestResultLogic
{
    public class TestResultService
    {
        private readonly IRepository<Test> _testEntities;
        private readonly CrawlerService _mainService;

        public TestResultService(
            CrawlerService mainService,
            IRepository<Test> testEntities)
        {
            _mainService = mainService;
            _testEntities = testEntities;
        }

        public void SaveToDatabase(string linkToScan, IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var urlEntity = MappedResultLinks(urlModels, urlResponseModels);

            _testEntities.AddAsync(new Test
                { Link = linkToScan, UrlWithResponseEntities = urlEntity });
            _testEntities.SaveChangesAsync();
        }

        private ICollection<TestResult> MappedResultLinks(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var resultLinks = new List<TestResult>();

            foreach (var entity in urlModels)
            {
                var timeResponse = urlResponseModels
                    .First(link => string.Equals(link.Link, entity.Link))
                    .TimeOfResponse;
                resultLinks.Add(
                    new TestResult
                    { Link = entity.Link, IsSitemap = entity.IsSitemap, IsWeb = entity.IsWeb, TimeOfResponse = timeResponse });
            }

            return resultLinks;
        }

        public virtual IEnumerable<TestDto> GetTestedLinks(int pageNumber, int pageSize)
        {
            var testedLinks = _testEntities
                .GetAll()
                .OrderBy(link => link.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var testResponse = testedLinks.Select(link => new TestDto
            {
                Id = link.Id,
                Link = link.Link,
                CreateAt = link.CreateAt
            });

            return testResponse;
        }

        public int GetTotalCount()
        {
            return _testEntities.GetAll().Count();
        }

        public virtual IEnumerable<TestResultDto> MappedTestedLinks(string link)
        {
            var testedLinks = GetLinksFromCrawler(link);
            var testedLinkWithResponse = GetLinksFromCrawlerWithResponse(testedLinks);

            SaveToDatabase(link, testedLinks, testedLinkWithResponse);
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
            var testedLink = _testEntities
                .Include(link => link.UrlWithResponseEntities)
                .FirstOrDefault(link => link.Id == id);

            var testResultResponse = testedLink.UrlWithResponseEntities
                .Select(link => new TestResultDto
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
