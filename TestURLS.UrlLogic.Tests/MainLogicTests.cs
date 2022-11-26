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
        private Mock<IUrlSettings> _urlSettings;
        private Mock<ITimeTracker> _timeTracker;

        /*string fakeHTML = @"<a href=""https://test.crawler.com/Info"">Link1</a>
                            <a href=""https://test.crawler.com/main.html"">Link2</a>";*/

        [SetUp]
        public void Setup()
        {
            _scanByHtml = new Mock<ILogicScanByHtml>();
            _scanBySitemap = new Mock<ILogicScanBySitemap>();
            _urlSettings = new Mock<IUrlSettings>();
            _timeTracker = new Mock<ITimeTracker>();

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
            var fakeUrl = "http://test.crawler.com/";
            var domainName = "http://test.crawler.com";
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
            var fakeUrl = "http://test.crawler.com/";
            var domainName = "http://test.crawler.com";
            var linksFromWeb = GetStartDataModel();
            var linksFromSitemap = GetDataFromSitemap();
            var linksFromWebWithSitemap = GetLinksExistBothScan();

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

        private List<UrlModel> GetStartDataModel()
        {
            var expectedLinks = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsWeb=true }
            };

            return expectedLinks;
        }

        private List<string> GetDataFromSitemap()
        {
            var expectedLinks = new List<string>()
            {
                "https://test.crawler.com/Info"
            };

            return expectedLinks;
        }

        private List<UrlModel> GetLinksExistBothScan()
        {
            var expectedLinks = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsSitemap=true, IsWeb=true }
            };

            return expectedLinks;
        }
    }
}
