using NUnit.Framework;

namespace TestURLS.UrlLogic.Tests
{
    public class StringServiceTests
    {
        private StringService _urlSettings;

        [SetUp]
        public void Setup()
        {
            _urlSettings = new StringService();
        }

        [Test]
        public void GetDomainName_DomainName_ExistLink()
        {
            var url = "https://test.crawler.com";
            //act
            var results = _urlSettings.GetDomainName(url);
            //assert
            Assert.AreEqual(url, results);
        }

        [Test]
        public void GetDomainName_Url_DomainNameFromUrl()
        {
            //arrange
            var url = "https://test.crawler.com/Info/";
            var expectedRsult = "https://test.crawler.com";
            //
            var results = _urlSettings.GetDomainName(url);
            //assert
            Assert.AreEqual(expectedRsult, results);
        }

        [Test]
        public void GetValidUrl_ValidUrl_Url()
        {
            //arrange
            var url = "https://test.crawler.com/Info/";
            var domainName = "https://test.crawler.com";
            //
            var result = _urlSettings.GetValidUrl(url, domainName);
            //assert
            Assert.AreEqual(url, result);
        }

        [Test]
        public void GetValidUrl_WrongUrl_NewUrl()
        {
            //arrange
            var url = "/Info/";
            var domainName = "https://test.crawler.com";
            var expectedResult = "https://test.crawler.com/Info/";
            //
            var result = _urlSettings.GetValidUrl(url, domainName);
            //assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetUrlLikeWeb_UncorrectUrl_NewUrl()
        {
            //arrange
            var url = "https://www.test.crawler.com/Info/";
            var domainName = "https://test.crawler.com";
            var expectedResult = "https://test.crawler.com/Info/";
            //
            var result = _urlSettings.GetUrlLikeFromWeb(url, domainName);
            //assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetUrlLikeWeb_CorrectUrl_ExistUrl()
        {
            //arrange
            var url = "https://test.crawler.com/Info/";
            var domainName = "https://test.crawler.com";
            //
            var result = _urlSettings.GetUrlLikeFromWeb(url, domainName);
            //assert
            Assert.AreEqual(url, result);
        }
    }
}