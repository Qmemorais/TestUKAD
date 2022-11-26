using NUnit.Framework;

namespace TestURLS.UrlLogic.Tests
{
    public class UrlSettingsTests
    {
        private ChangesAboveLink _urlSettings;

        [SetUp]
        public void Setup()
        {
            _urlSettings = new ChangesAboveLink();
        }

        [Test]
        public void GetDomainName_DomainName_ExistLink()
        {
            var url = "https://test.crawler.com";
            //
            var results = _urlSettings.GetDomainName(url);
            //
            Assert.AreEqual(url, results);

        }

        [Test]
        public void GetDomainName_Url_DomainNameFromUrl()
        {
            var url = "https://test.crawler.com/Info/";
            var expectedRsult = "https://test.crawler.com";
            //
            var results = _urlSettings.GetDomainName(url);
            //
            Assert.AreEqual(expectedRsult, results);
        }

        [Test]
        public void GetValidUrl_ValidUrl_Url()
        {
            //
            var url = "https://test.crawler.com/Info/";
            var domainName = "https://test.crawler.com";
            //
            var result = _urlSettings.GetValidUrl(url, domainName);
            //
            Assert.AreEqual(url, result);
        }

        [Test]
        public void GetValidUrl_WrongUrl_NewUrl()
        {
            //
            var url = "/Info/";
            var domainName = "https://test.crawler.com";
            var expectedResult = "https://test.crawler.com/Info/";
            //
            var result = _urlSettings.GetValidUrl(url, domainName);
            //
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetUrlLikeWeb_UncorrectUrl_NewUrl()
        {
            //
            var url = "https://www.test.crawler.com/Info/";
            var domainName = "https://test.crawler.com";
            var expectedResult = "https://test.crawler.com/Info/";
            //
            var result = _urlSettings.GetUrlLikeFromWeb(url, domainName);
            //
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetUrlLikeWeb_CorrectUrl_ExistUrl()
        {
            //
            var url = "https://test.crawler.com/Info/";
            var domainName = "https://test.crawler.com";
            //
            var result = _urlSettings.GetUrlLikeFromWeb(url, domainName);
            //
            Assert.AreEqual(url, result);
        }
    }
}