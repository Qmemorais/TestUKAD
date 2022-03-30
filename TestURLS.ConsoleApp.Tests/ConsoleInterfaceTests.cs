using System.Collections.Generic;
using System.Data;
using System.Net;
using Moq;
using NUnit.Framework;
using TestUrls.BusinessLogic;
using TestUrls.EntityFramework.Entities;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp.Tests
{
    public class LogicToConsoleTests
    {
        private Mock<ConsoleInOut> _consoleInOut;
        private Mock<MainService> _mainLogic;
        private Mock<WebService> _webService;
        private Mock<SitemapService> _sitemapService;
        private Mock<StringService> _stringService;
        private Mock<HttpService> _httpService;
        private Mock<ResponseService> _responseService;
        private Mock<IRepository<Test>> _testEntities;
        private Mock<OutputToConsole> _outputToConsole;
        private LogicToConsole _consoleInterface;
        private Mock<BusinessService> _businessService;
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

            _mainLogic = new Mock<MainService>(_webService.Object,_sitemapService.Object,_stringService.Object,
                _responseService.Object);
            _businessService = new Mock<BusinessService>(_mainLogic.Object, _testEntities.Object);
            _outputToConsole = new Mock<OutputToConsole>(_consoleInOut.Object);
            _consoleInterface = new LogicToConsole(
                _consoleInOut.Object,
                _businessService.Object,
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
            _businessService
                .Setup(x => x.GetLinksFromCrawler(""))
                .Throws(new WebException(writeLineRes));
            //assert
            WebException ex = Assert.Throws<WebException>(() => _consoleInterface.Start());
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
            _businessService
                .Setup(x => x.GetLinksFromCrawler(readLine))
                .Throws(new WebException(writeLineRes));
            //assert
            WebException ex = Assert.Throws<WebException>(() => _consoleInterface.Start());
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
            _businessService
                .Setup(x => x.GetLinksFromCrawler(readLine))
                .Throws(new WebException(writeLineRes));
            //assert
            WebException ex = Assert.Throws<WebException>(() => _consoleInterface.Start());
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
            _businessService
                .Setup(x => x.GetLinksFromCrawler(fakeUrl))
                .Returns(expectedUrl);
            _businessService
                .Setup(x => x.GetLinksFromCrawlerWithResponse(expectedUrl))
                .Returns(expectedUrlWithTime);
            //act
            _consoleInterface.Start();
            //assert
            _outputToConsole.Verify(x => x.WriteLinksWithoutTime(expectedUrl), Times.Once);
            _outputToConsole.Verify(x => x.WriteLinksWithTime(expectedUrlWithTime), Times.Once);
            _consoleInOut.Verify(x => x.Write(_writeLineOutput), Times.Once);
        }
    }
}