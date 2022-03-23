using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface IMainLogic
    {
        List<UrlModel> GetResults(string url);
        IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(List<UrlModel> htmlToGetTime);
    }
}
