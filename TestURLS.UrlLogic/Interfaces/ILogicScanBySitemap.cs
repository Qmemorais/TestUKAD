using System.Collections.Generic;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface ILogicScanBySitemap
    {
        IEnumerable<string> GetLinksFromSitemapIfExist(string url);
    }
}
