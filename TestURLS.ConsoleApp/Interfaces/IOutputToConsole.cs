using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.ConsoleApp.Interfaces
{
    public interface IOutputToConsole
    {
        void WriteLinksWithoutTime(List<UrlModel> allLinksFromSitemapAndScan);
        void WriteLinksWithTime(List<UrlModelWithResponse> linksWithResponseTime);
        void WriteCountLinks(List<UrlModel> allLinksFromSitemapAndScan);
    }
}
