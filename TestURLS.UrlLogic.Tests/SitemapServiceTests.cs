using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace TestURLS.UrlLogic.Tests
{
    public class SitemapServiceTests
    {
        private SitemapService _sitemapService;
        private Mock<StringService> _stringService;
        private Mock<HttpService> _httpService;

        [SetUp]
        public void Setup()
        {
            _stringService = new Mock<StringService>();
            _httpService = new Mock<HttpService>();

            _sitemapService = new SitemapService(
                _httpService.Object,
                _stringService.Object);
        }

        [Test]
        public void GetLinksFromSitemap_InvalidUrl_EmptyList()
        {
            //arrange
            var invalidUrl = "test.crawler";
            var domainName = "test.crawler";
            var expectedLinks = new List<string>();

            _httpService
                .Setup(getBody => getBody.GetBodyFromUrl(invalidUrl))
                .Returns(string.Empty);
            _stringService
                .Setup(getDomain => getDomain.GetDomainName(invalidUrl))
                .Returns(domainName);
            //
            var actualResults = _sitemapService.GetLinksFromSitemapIfExist(invalidUrl);
            //assert
            Assert.AreEqual(expectedLinks, actualResults);
            Assert.AreEqual(expectedLinks.Count, actualResults.Count());
        }

        [Test]
        public void GetLinksFromSitemap_ValidUrl_ListOfLinks()
        {
            //arrange
            var validUrl = "https://test.crawler.com/sitemap.xml";
            var domainName = "https://test.crawler.com";
            var expectedLinks = new List<string>
            {
                "https://test.crawler.com/Info/"
            };
            var fakeXml = @"
                <urlset>
                    <url>
                        <loc>https://test.crawler.com/Info/</loc>
                        <lastmod></lastmod>
                    </url>
                </urlset>";

            _httpService
                .Setup(getBody => getBody.GetBodyFromUrl(validUrl))
                .Returns(fakeXml);
            _stringService
                .Setup(getDomain => getDomain.GetDomainName(validUrl))
                .Returns(domainName);
            //
            var actualResults = _sitemapService.GetLinksFromSitemapIfExist(validUrl);
            //assert
            Assert.AreEqual(expectedLinks, actualResults);
            Assert.AreEqual(expectedLinks.Count, actualResults.Count());
        }
    }
}
