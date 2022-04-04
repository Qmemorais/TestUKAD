using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Tests
{
    public class MainServiceTests
    {
        private CrawlerService _mainLogic;
        private Mock<WebService> _webService;
        private Mock<SitemapService> _sitemapService;
        private Mock<StringService> _stringService;
        private Mock<HttpService> _httpService;
        private Mock<ResponseService> _responseServer;

        [SetUp]
        public void Setup()
        {
            _stringService = new Mock<StringService>();
            _httpService = new Mock<HttpService>();
            _responseServer = new Mock<ResponseService>();
            _webService = new Mock<WebService>(_stringService.Object, _httpService.Object);
            _sitemapService = new Mock<SitemapService>(_httpService.Object, _stringService.Object);

            _mainLogic = new CrawlerService(
                _webService.Object,
                _sitemapService.Object,
                _stringService.Object,
                _responseServer.Object);
        }

        [Test]
        public void GetResults_OnlyScanWeb_LinksFromWeb()
        {
            //arrange
            var fakeUrl = "https://test.crawler.com/";
            var domainName = "https://test.crawler.com";
            var linkFromScanPage = new List<string>()
            {
                "https://test.crawler.com/Info"
            };
            var expectedLinks = new List<UrlModel>()
            {
                new UrlModel{Link = "https://test.crawler.com/Info", IsWeb=true}
            };

            _webService
                .Setup(getLinks => getLinks.GetUrlsFromScanPages(fakeUrl))
                .Returns(linkFromScanPage);
            _sitemapService
                .Setup(getLinks => getLinks.GetLinksFromSitemapIfExist(fakeUrl))
                .Returns(new List<string>());
            _stringService
                .Setup(getDomain => getDomain.GetDomainName(fakeUrl))
                .Returns(domainName);
            //act
            var result = _mainLogic.GetResults(fakeUrl);
            //assert
            Assert.AreEqual(expectedLinks.Count, result.Count());
            Assert.AreEqual(expectedLinks.FirstOrDefault().IsSitemap, result.FirstOrDefault().IsSitemap);
        }

        [Test]
        public void GetResults_LinksFromScanAndWeb_LinksWithScanAndWeb()
        {
            //arrange
            var fakeUrl = "https://test.crawler.com/";
            var domainName = "https://test.crawler.com";
            var linksFromWeb = new List<string>()
            {
                "https://test.crawler.com/Info"
            };
            var linksFromSitemap = new List<string>()
            {
                "https://test.crawler.com/Info"
            };
            var linksFromWebWithSitemap = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsSitemap=true, IsWeb=true }
            };

            _webService
                .Setup(getLinks => getLinks.GetUrlsFromScanPages(fakeUrl))
                .Returns(linksFromWeb);
            _sitemapService
                .Setup(getLinks => getLinks.GetLinksFromSitemapIfExist(fakeUrl))
                .Returns(linksFromSitemap);
            _stringService
                .Setup(getDomain => getDomain.GetDomainName(linksFromWeb.FirstOrDefault()))
                .Returns(domainName);
            _stringService
                .Setup(getValid => getValid.GetUrlLikeFromWeb(linksFromSitemap.FirstOrDefault(), domainName))
                .Returns(linksFromSitemap.FirstOrDefault());
            //act
            var result = _mainLogic.GetResults(fakeUrl);
            //assert
            Assert.AreEqual(linksFromWebWithSitemap.Count, result.Count());
        }

        [Test]
        public void GetUrlsWithTimeResponse_NotNullList_NewListWithResponse()
        {
            //arrange
            var modelToGetTime = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsSitemap=true, IsWeb=true }
            };
            var modelWithTime = new List<UrlModelWithResponse>()
            {
                new UrlModelWithResponse{Link="https://test.crawler.com/Info", TimeOfResponse=14  }
            };

            _responseServer
                .Setup(getTime => getTime.GetLinksWithTime(modelToGetTime))
                .Returns(modelWithTime);
            //
            var results = _mainLogic.GetUrlsWithTimeResponse(modelToGetTime);
            //assert
            Assert.AreEqual(modelWithTime, results);
        }
    }
}
