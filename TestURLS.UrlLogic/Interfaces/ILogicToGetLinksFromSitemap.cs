using System.Collections.Generic;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface ILogicToGetLinksFromSitemap
    {
        IEnumerable<string> GetLinksFromSitemapIfExist(string url);
    }
}
