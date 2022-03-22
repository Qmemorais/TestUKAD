using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface ILogicToGetLinksFromScanWeb
    {
        IEnumerable<UrlModel> GetUrlsFromScanPages(string url);
    }
}
