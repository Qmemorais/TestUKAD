using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp.Interfaces
{
    public interface IOutputToConsole
    {
        void WriteLinksWithoutTime(IEnumerable<UrlModel> allLinksFromSitemapAndScan);
        void WriteLinksWithTime(IEnumerable<UrlModelWithResponse> linksWithResponseTime);
        void WriteCountLinks(IEnumerable<UrlModel> allLinksFromSitemapAndScan);
    }
}
