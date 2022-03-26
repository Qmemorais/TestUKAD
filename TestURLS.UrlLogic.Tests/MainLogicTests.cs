using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using TestUrls.EntityFramework.UnitOfWorkPatern;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Tests
{
    public class MainLogicTests
    {
        private MainLogic _mainLogic;
        private Mock<ILogicToGetLinksFromScanWeb> _scanByHtml;
        private Mock<ILogicToGetLinksFromSitemap> _scanBySitemap;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ChangesAboveLink> _urlSettings;
        private Mock<ResponseTimeOfUrl> _timeTracker;

        [SetUp]
        public void Setup()
        {
            _scanByHtml = new Mock<ILogicToGetLinksFromScanWeb>();
            _scanBySitemap = new Mock<ILogicToGetLinksFromSitemap>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _urlSettings = new Mock<ChangesAboveLink>();
            _timeTracker = new Mock<ResponseTimeOfUrl>();

            _mainLogic = new MainLogic(
                _scanByHtml.Object,
                _scanBySitemap.Object,
                _unitOfWork.Object,
                _urlSettings.Object,
                _timeTracker.Object);
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

            _scanByHtml
                .Setup(getLinks => getLinks.GetUrlsFromScanPages(fakeUrl))
                .Returns(linkFromScanPage);
            _scanBySitemap
                .Setup(getLinks => getLinks.GetLinksFromSitemapIfExist(fakeUrl))
                .Returns(new List<string>());
            _urlSettings
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

            _scanByHtml
                .Setup(getLinks => getLinks.GetUrlsFromScanPages(fakeUrl))
                .Returns(linksFromWeb);
            _scanBySitemap
                .Setup(getLinks => getLinks.GetLinksFromSitemapIfExist(fakeUrl))
                .Returns(linksFromSitemap);
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(linksFromWeb.FirstOrDefault()))
                .Returns(domainName);
            _urlSettings
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
            //assert
            var modelToGetTime = new List<UrlModel>()
            {
                new UrlModel{Link="https://test.crawler.com/Info", IsSitemap=true, IsWeb=true }
            };
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
    }
}
