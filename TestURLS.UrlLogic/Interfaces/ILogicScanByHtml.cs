using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface ILogicScanByHtml
    {
        IEnumerable<UrlModel> GetUrlsFromScanPages(string url);
    }
}
