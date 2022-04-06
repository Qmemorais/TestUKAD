using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moq;
using NUnit.Framework;

namespace TestURLS.UrlLogic.Tests
{
    public class WebServiceTests
    {
        private WebService _webService;
        private Mock<StringService> _stringService;
        private Mock<HttpService> _httpService;

        [SetUp]
        public void Setup()
        {
            _stringService = new Mock<StringService>();
            _httpService = new Mock<HttpService>();

            _webService = new WebService(
                _stringService.Object,
                _httpService.Object);
        }

        [Test]
        public void GetUrlsFromScanPages_InvalidString_WebExceptionMessage()
        {
            //arrange
            var invalidUrl = "hahahah";
            var domainName = "hahahah";
            var writeLineRes = "Invalid URI: The URI is empty.";

            _httpService
                .Setup(getBody => getBody.GetBodyFromUrl(invalidUrl))
                .Throws(new WebException(writeLineRes));
            _stringService
                .Setup(getDomain => getDomain.GetDomainName(invalidUrl))
                .Returns(domainName);
            //assert
            WebException ex = Assert.Throws<WebException>(() => _webService.GetUrlsFromScanPages(invalidUrl));
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

            _httpService
                .Setup(getBody => getBody.GetBodyFromUrl(validUrl))
                .Returns(fakeHtml);
            _stringService
                .Setup(getDomain => getDomain.GetDomainName(validUrl))
                .Returns(domainName);
            _stringService
                .Setup(getValid => getValid.GetValidUrl(urlToGetValidUrl, domainName))
                .Returns(urlToGetValidUrl);
            //
            var actualResult = _webService.GetUrlsFromScanPages(validUrl);
            //assert
            Assert.AreEqual(expectedResult.Count, actualResult.Count());
        }
    }
}
