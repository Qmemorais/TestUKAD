using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Tests
{
    public class MainLogicTests
    {
        private MainLogic _mainLogic;
        private Mock<ILogicScanByHtml> _scanByHtml;
        private Mock<ILogicScanBySitemap> _scanBySitemap;
        private Mock<UrlSettings> _urlSettings;
        private Mock<TimeTracker> _timeTracker;

        [SetUp]
        public void Setup()
        {
            _scanByHtml = new Mock<ILogicScanByHtml>();
            _scanBySitemap = new Mock<ILogicScanBySitemap>();
            _urlSettings = new Mock<UrlSettings>();
            _timeTracker = new Mock<TimeTracker>();

            _mainLogic = new MainLogic(
                _scanByHtml.Object,
                _scanBySitemap.Object,
                _urlSettings.Object,
                _timeTracker.Object);
        }

        [Test]
        public void GetResults_OnlyScanWeb_LinksFromWeb()
        {
            //arrange
            var fakeUrl = "https://test.crawler.com/";
            var domainName = "https://test.crawler.com";
            var expectedLinks = GetStartDataModel();

            _scanByHtml
                .Setup(getLinks => getLinks.GetUrlsFromScanPages(fakeUrl))
                .Returns(expectedLinks);
            _scanBySitemap
                .Setup(getLinks => getLinks.GetLinksFromSitemapIfExist(fakeUrl))
                .Returns(new List<string>());
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(fakeUrl))
                .Returns(domainName);
            //act
            var result = _mainLogic.GetResults(fakeUrl);
            //assert
            Assert.AreEqual(expectedLinks, result);
        }

        [Test]
        public void GetResults_LinksFromScanAndWeb_LinksWithScanAndWeb()
        {
            //arrange
            var fakeUrl = "https://test.crawler.com/";
            var domainName = "https://test.crawler.com";
            var linksFromWeb = GetStartDataModel();
            var linksFromSitemap = new List<string>()
            {
                "https://test.crawler.com/Info"
            };
            var linksFromWebWithSitemap = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsSitemap=true, IsWeb=true }
            };

            _scanByHtml
                .Setup(getLinks => getLinks.GetUrlsFromScanPages(fakeUrl))
                .Returns(linksFromWeb);
            _scanBySitemap
                .Setup(getLinks => getLinks.GetLinksFromSitemapIfExist(fakeUrl))
                .Returns(linksFromSitemap);
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(linksFromWeb.FirstOrDefault().Link))
                .Returns(domainName);
            _urlSettings
                .Setup(getValid => getValid.GetUrlLikeFromWeb(linksFromSitemap.FirstOrDefault(), domainName))
                .Returns(linksFromSitemap.FirstOrDefault());
            //act
            var result = _mainLogic.GetResults(fakeUrl);
            //assert
            Assert.AreEqual(linksFromWebWithSitemap.Count, result.Count);
        }

        [Test]
        public void GetUrlsWithTimeResponse_NotNullList_NewListWithResponse()
        {
            //assert
            var modelToGetTime = GetStartDataModel();
            var modelWithTime = new List<UrlModelWithResponse>()
            {
                new UrlModelWithResponse{Link="https://test.crawler.com/Info", TimeOfResponse=14  }
            };

            _timeTracker
                .Setup(getTime => getTime.GetLinksWithTime(modelToGetTime))
                .Returns(modelWithTime);
            //
            var results = _mainLogic.GetUrlsWithTimeResponse(modelToGetTime);
            //
            Assert.AreEqual(modelWithTime, results);

        }

        private List<UrlModel> GetStartDataModel()
        {
            var expectedLinks = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsWeb=true }
            };

            return expectedLinks;
        }
    }
}
