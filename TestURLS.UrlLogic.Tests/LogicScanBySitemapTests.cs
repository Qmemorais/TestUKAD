using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic.Tests
{
    public class LogicScanBySitemapTests
    {
        private LogicScanBySitemap _logicSitemap;
        private Mock<IUrlSettings> _urlSettings;
        private Mock<IHttpLogic> _getHttp;

        [SetUp]
        public void Setup()
        {
            _urlSettings = new Mock<IUrlSettings>();
            _getHttp = new Mock<IHttpLogic>();

            _logicSitemap = new LogicScanBySitemap(
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
            var fakeXml = new StringBuilder();
            fakeXml.Append("<urlset>");
            fakeXml.Append("<url>");
            fakeXml.Append("<loc>https://test.crawler.com/Info/</loc>");
            fakeXml.Append("<lastmod> 2022 - 01 - 19T16: 03:22 + 01:00 </lastmod>");
            fakeXml.Append("</url>");
            fakeXml.Append("</urlset>");

            _getHttp
                .Setup(getBody => getBody.GetBodyFromUrl(validUrl))
                .Returns(fakeXml.ToString());
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
