using System.Collections.Generic;
using System.Net;
using Moq;
using NUnit.Framework;
using TestURLS.ConsoleApp.Interfaces;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp.Tests
{
    public class LogicToConsoleTests
    {
        private Mock<IConsoleInOut> _consoleInOut;
        private Mock<IMainLogic> _mainLogic;
        private Mock<IOutputToConsole> _outputToConsole;
        private LogicToConsole _consoleInterface;
        private readonly string _writeLineOutput = "Press <Enter>";

        [SetUp]
        public void Setup()
        {
            _consoleInOut = new Mock<IConsoleInOut>();
            _mainLogic = new Mock<IMainLogic>();
            _outputToConsole = new Mock<IOutputToConsole>();
            _consoleInterface = new LogicToConsole(
                _consoleInOut.Object,
                _mainLogic.Object,
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
            _mainLogic
                .Setup(x => x.GetResults(""))
                .Throws(new WebException(writeLineRes));
            //assert
            Assert.Throws<WebException>(() => _consoleInterface.Start()).Message.Equals(writeLineRes);
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
            _mainLogic
                .Setup(x => x.GetResults(readLine))
                .Throws(new WebException(writeLineRes));
            //assert
            Assert.Throws<WebException>(() => _consoleInterface.Start()).Message.Equals(writeLineRes);
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
            _mainLogic
                .Setup(x => x.GetResults(readLine))
                .Throws(new WebException(writeLineRes));
            //assert
            Assert.Throws<WebException>(() => _consoleInterface.Start()).Message.Equals(writeLineRes);
        }

        [Test]
        public void Start_InputCorrectUrl_ReturnCorectCountWriteLine()
        {
            //arrange
            var fakeURL = "https://example.com/";
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
                .Returns(fakeURL);
            _mainLogic
                .Setup(x => x.GetResults(fakeURL))
                .Returns(expectedUrl);
            _mainLogic
                .Setup(x => x.GetUrlsWithTimeResponse(expectedUrl))
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