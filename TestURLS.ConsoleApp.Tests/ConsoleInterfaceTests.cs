using NUnit.Framework;
using Moq;
using TestURLS.UrlLogic;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace TestURLS.ConsoleApp.Tests
{
    public class Tests
    {
        private Mock<IConsoleInOut> _consoleInOut;
        private Mock<MainLogic> _mainLogic;
        private ConsoleInterface _consoleInterface;
        private readonly string _writeLineOutput = "Press <Enter>";

        [SetUp]
        public void Setup()
        {
            _consoleInOut = new Mock<IConsoleInOut>();
            _mainLogic = new Mock<MainLogic>();
            _consoleInterface = new ConsoleInterface(_consoleInOut.Object,_mainLogic.Object);
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
                .Throws(new WebException("Invalid URI: The URI is empty."));
            //act
            _consoleInterface.Start();
            //assert
            _consoleInOut.Verify(x => x.Write(writeLineRes), Times.Once);
            _consoleInOut.Verify(x => x.Write(_writeLineOutput), Times.Once);
            _consoleInOut.Verify(x => x.Read(), Times.Exactly(2));
        }

        [Test]
        public void Start_InputCorrectURL_ReturnExceptionMessageForbiden()
        {
            //arrange
            var readLine = "";
            var writeLineRes = "403 - Forbidden";

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(readLine);
            _mainLogic
                .Setup(x => x.GetResults(""))
                .Throws(new WebException("403 - Forbidden"));
            //act
            _consoleInterface.Start();
            //assert
            _consoleInOut.Verify(x => x.Write(writeLineRes), Times.Once);
            _consoleInOut.Verify(x => x.Write(_writeLineOutput), Times.Once);
            _consoleInOut.Verify(x => x.Read(), Times.Exactly(2));
        }

        [Test]
        public void Start_InputCorrectURL_ReturnExceptionMessageNotFound()
        {
            //arrange
            var readLine = "";
            var writeLineRes = "404 - NotFound";

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(readLine);
            _mainLogic
                .Setup(x => x.GetResults(""))
                .Throws(new WebException("404 - NotFound"));
            //act
            _consoleInterface.Start();
            //assert
            _consoleInOut.Verify(x => x.Write(writeLineRes), Times.Once);
            _consoleInOut.Verify(x => x.Write(_writeLineOutput), Times.Once);
            _consoleInOut.Verify(x => x.Read(), Times.Exactly(2));
        }

        [Test]
        public void Start_InputCorrectURL_ReturnCorectCountWriteLine()
        {
            //arrange
            var fakeURL = "https://example.com/";
            var expectedURL = new List<string>()
            {
                "https://test.crawler.com/Info",
                "https://test.crawler.com/main.html"
            };

            _consoleInOut
                .Setup(x => x.Read())
                .Returns(fakeURL);
            _mainLogic
                .Setup(x => x.GetResults(fakeURL))
                .Returns(expectedURL);
            //act
            _consoleInterface.Start();
            //assert
            _consoleInOut.Verify(x => x.Write(expectedURL.First()), Times.Once);
            _consoleInOut.Verify(x => x.Write(_writeLineOutput), Times.Once);
        }
    }
}