using Moq;
using System.Net;
using NUnit.Framework;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TestURLS.UrlLogic.Tests
{
    public class MainLogicTests
    {
        private MainLogic _mainLogic;
        private Mock<HttpWebResponse> _response;
        private Mock<HttpWebRequest> _request;
        private Mock<LogicScanByHTML> _scanByHTML;
        private Mock<LogicScanBySitemap> _scanBySitemap;

        [SetUp]
        public void Setup()
        {
            _response = new Mock<HttpWebResponse>();
            _request = new Mock<HttpWebRequest>();
            _scanByHTML = new Mock<LogicScanByHTML>();
            _scanBySitemap = new Mock<LogicScanBySitemap>();
            _mainLogic = new MainLogic(_scanByHTML.Object, _scanBySitemap.Object);
        }

        [Test]
        public void Test1()
        {
            //
            var fakeURL = new List<string>()
            {
                "http://test.crawler.com/"
            };
            var fakeHTML = @"<a href=""https://test.crawler.com/Info"">Link1</a>
                            <a href=""https://test.crawler.com/main.html"">Link2</a>";
            var expectedHTML = new List<string>()
            {
                "https://test.crawler.com/Info",
                "https://test.crawler.com/main.html"
            };

            var expectedBytes = Encoding.UTF8.GetBytes(fakeHTML);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            _response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            _request.Setup(c => c.GetResponse()).Returns(_response.Object);

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(_request.Object);

            // act
            var actualRequest = factory.Object.Create(fakeURL.First());
            actualRequest.Method = WebRequestMethods.Http.Get;

            _request.Setup(x => x.GetResponse()).Returns(actualRequest.GetResponse());

            _scanByHTML
                .Setup(x => x.ScanWebPages(fakeURL))
                .Returns(expectedHTML);
            _scanBySitemap
                .Setup(x => x.ScanExistSitemap(fakeURL.First(), fakeURL))
                .Returns(expectedHTML);
            //
            //
            var result = _mainLogic.GetResults(fakeURL.First());
            Assert.AreEqual(true, result);
        }

        public interface IHttpWebRequestFactory
        {
            HttpWebRequest Create(string uri);
        }

    }
}
