using System.Collections.Generic;
using System.Data;
using System.Net;
using Moq;
using NUnit.Framework;
using TestUrls.EntityFramework.Entities;
using TestUrls.TestResultLogic;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp.Tests
{
    public class LogicToConsoleTests
    {
        private Mock<ConsoleInOut> _consoleInOut;
        private Mock<CrawlerService> _crawlerService;
        private Mock<WebService> _webService;
        private Mock<SitemapService> _sitemapService;
        private Mock<StringService> _stringService;
        private Mock<HttpService> _httpService;
        private Mock<ResponseService> _responseService;
        private Mock<IRepository<Test>> _testEntities;
        private Mock<OutputToConsole> _outputToConsole;
        private LogicToConsole _logicToConsole;
        private Mock<TestResultService> _testResultService;
        private readonly string _writeLineOutput = "Press <Enter>";

        [SetUp]
        public void Setup()
        {
            _consoleInOut = new Mock<ConsoleInOut>();
            _httpService = new Mock<HttpService>();
            _testEntities = new Mock<IRepository<Test>>();
            _stringService = new Mock<StringService>();
            _responseService = new Mock<ResponseService>();
            _webService = new Mock<WebService>(_stringService.Object, _httpService.Object);
            _sitemapService = new Mock<SitemapService>(_httpService.Object, _stringService.Object);

            _crawlerService = new Mock<CrawlerService>(_webService.Object,_sitemapService.Object,_stringService.Object,
                _responseService.Object);
            _testResultService = new Mock<TestResultService>(_crawlerService.Object, _testEntities.Object);
            _outputToConsole = new Mock<OutputToConsole>(_consoleInOut.Object);
            _logicToConsole = new LogicToConsole(
                _consoleInOut.Object,
                _testResultService.Object,
                _outputToConsole.Object);
        }

        [Test]
        public void Start_InputEmptyString_ReturnExceptionMessage()
        {
            //arrange
            var readLine = "";
            var writeLineRes = "Invalid URI: The URI is empty.";

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(readLine);
            _testResultService
                .Setup(x => x.GetLinksFromCrawler(""))
                .Throws(new WebException(writeLineRes));
            //assert
            WebException ex = Assert.Throws<WebException>(() => _logicToConsole.Start());
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(writeLineRes));
        }

        [Test]
        public void Start_InputCorrectUrl_ReturnExceptionMessageForbiden()
        {
            //arrange
            var readLine = "https://example.com/";
            var writeLineRes = "403 - Forbidden";

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(readLine);
            _testResultService
                .Setup(x => x.GetLinksFromCrawler(readLine))
                .Throws(new WebException(writeLineRes));
            //assert
            WebException ex = Assert.Throws<WebException>(() => _logicToConsole.Start());
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(writeLineRes));
        }

        [Test]
        public void Start_InputCorrectUrl_ReturnExceptionMessageNotFound()
        {
            //arrange
            var readLine = "https://example.com/";
            var writeLineRes = "404 - NotFound";

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(readLine);
            _testResultService
                .Setup(x => x.GetLinksFromCrawler(readLine))
                .Throws(new WebException(writeLineRes));
            //assert
            WebException ex = Assert.Throws<WebException>(() => _logicToConsole.Start());
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(writeLineRes));
        }

        [Test]
        public void Start_InputCorrectUrl_ReturnCorectCountWriteLine()
        {
            //arrange
            var fakeUrl = "https://example.com/";
            var expectedUrl = new List<UrlModel>()
            {
                new UrlModel{Link ="https://test.crawler.com/Info",IsWeb=true},
                new UrlModel{Link ="https://test.crawler.com/main.html",IsSitemap=true}
            };
            var expectedUrlWithTime = new List<UrlModelWithResponse>()
            {
                new UrlModelWithResponse{Link ="https://test.crawler.com/Info", TimeOfResponse=12},
                new UrlModelWithResponse{Link ="https://test.crawler.com/main.html",TimeOfResponse=4}
            };

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(fakeUrl);
            _testResultService
                .Setup(x => x.GetLinksFromCrawler(fakeUrl))
                .Returns(expectedUrl);
            _testResultService
                .Setup(x => x.GetLinksFromCrawlerWithResponse(expectedUrl))
                .Returns(expectedUrlWithTime);
            //act
            _logicToConsole.Start();
            //assert
            _outputToConsole.Verify(x => x.WriteLinksWithoutTime(expectedUrl), Times.Once);
            _outputToConsole.Verify(x => x.WriteLinksWithTime(expectedUrlWithTime), Times.Once);
            _consoleInOut.Verify(x => x.Write(_writeLineOutput), Times.Once);
        }
    }
}