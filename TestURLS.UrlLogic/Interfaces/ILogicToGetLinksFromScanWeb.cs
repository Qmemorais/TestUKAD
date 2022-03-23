using System.Collections.Generic;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface ILogicToGetLinksFromScanWeb
    {
        IEnumerable<string> GetUrlsFromScanPages(string url);
    }
}
