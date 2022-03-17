using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Tests
{
    public class LogicScanByHtmlTests
    {
        private LogicScanByHtml _logicHtml;
        private Mock<IUrlSettings> _urlSettings;
        private Mock<IHttpLogic> _getHttp;

        [SetUp]
        public void Setup()
        {
            _urlSettings = new Mock<IUrlSettings>();
            _getHttp = new Mock<IHttpLogic>();

            _logicHtml = new LogicScanByHtml(
                _urlSettings.Object,
                _getHttp.Object);
        }

        [Test]
        public void GetUrlsFromScanPages_InvalidString_WebExceptionMeaasge()
        {
            //arrange
            var invalidUrl = "hahahah";
            var domainName = "hahahah";
            var writeLineRes = "Invalid URI: The URI is empty.";

            _getHttp
                .Setup(getBody => getBody.GetBodyFromUrl(invalidUrl))
                .Throws(new WebException(writeLineRes));
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(invalidUrl))
                .Returns(domainName);
            //assert
            WebException ex = Assert.Throws<WebException>(() => _logicHtml.GetUrlsFromScanPages(invalidUrl));
            Assert.That(ex.Message, Is.EqualTo(writeLineRes));
        }

        [Test]
        public void GetUrlsFromScanPages_ValidString_ListOfUrlModel()
        {
            //arrange
            var validUrl = "https://test.crawler.com/";
            var domainName = "https://test.crawler.com";
            string fakeHtml = @"<a href=""https://test.crawler.com/Info/"">Link1</a>";
            var expectedResult = new List<UrlModel>
            {
                new UrlModel{ Link="https://test.crawler.com/", IsWeb=true},
                new UrlModel{ Link="https://test.crawler.com/Info/", IsWeb=true}
            };

            _getHttp
                .Setup(getBody => getBody.GetBodyFromUrl(validUrl))
                .Returns(fakeHtml);
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(validUrl))
                .Returns(domainName);
            _urlSettings
                .Setup(getValid => getValid.GetValidUrl("https://test.crawler.com/Info/", domainName))
                .Returns("https://test.crawler.com/Info/");
            //
            var actualResult = _logicHtml.GetUrlsFromScanPages(validUrl);
            //assert
            Assert.AreEqual(expectedResult.Count, actualResult.Count());
        }
    }
}
