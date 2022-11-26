using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace TestURLS.UrlLogic.Tests
{
    public class LogicScanBySitemapTests
    {
        private LogicToGetLinksFromSitemap _logicSitemap;
        private Mock<ChangesAboveLink> _urlSettings;
        private Mock<HttpLogic> _getHttp;

        [SetUp]
        public void Setup()
        {
            _urlSettings = new Mock<ChangesAboveLink>();
            _getHttp = new Mock<HttpLogic>();

            _logicSitemap = new LogicToGetLinksFromSitemap(
                _getHttp.Object,
                _urlSettings.Object);
        }

        [Test]
        public void GetLinksFromSitemap_InvalidUrl_EmptyList()
        {
            //arrange
            var invalidUrl = "hahahah";
            var domainName = "hahahah";
            var expectedLinks = new List<string>();

            _getHttp
                .Setup(getBody => getBody.GetBodyFromUrl(invalidUrl))
                .Returns(string.Empty);
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(invalidUrl))
                .Returns(domainName);
            //
            var actualResults = _logicSitemap.GetLinksFromSitemapIfExist(invalidUrl);
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

            _getHttp
                .Setup(getBody => getBody.GetBodyFromUrl(validUrl))
                .Returns(fakeXml);
            _urlSettings
                .Setup(getDomain => getDomain.GetDomainName(validUrl))
                .Returns(domainName);
            //
            var actualResults = _logicSitemap.GetLinksFromSitemapIfExist(validUrl);
            //assert
            Assert.AreEqual(expectedLinks, actualResults);
            Assert.AreEqual(expectedLinks.Count, actualResults.Count());
        }
    }
}
