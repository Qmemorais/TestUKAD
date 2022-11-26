using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;

namespace TestURLS.UrlLogic.Tests
{
    public class WebServiceTests
    {
        private WebService _logicHtml;
        private Mock<StringService> _urlSettings;
        private Mock<HttpService> _getHttp;

        [SetUp]
        public void Setup()
        {
            _urlSettings = new Mock<StringService>();
            _getHttp = new Mock<HttpService>();

            _logicHtml = new WebService(
                _urlSettings.Object,
                _getHttp.Object);
        }

        [Test]
        public void GetUrlsFromScanPages_InvalidString_WebExceptionMessage()
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
            var expectedResult = new List<string>
            {
                "https://test.crawler.com/",
                "https://test.crawler.com/Info/"
            };
            var urlToGetValidUrl = "https://test.crawler.com/Info/";

            _getHttp
                .Setup(getBody => getBody.GetBodyFromUrl(validUrl))
                .Returns(fakeHtml);
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(validUrl))
                .Returns(domainName);
            _urlSettings
                .Setup(getValid => getValid.GetValidUrl(urlToGetValidUrl, domainName))
                .Returns(urlToGetValidUrl);
            //
            var actualResult = _logicHtml.GetUrlsFromScanPages(validUrl);
            //assert
            Assert.AreEqual(expectedResult.Count, actualResult.Count());
        }
    }
}
