using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface IMainLogic
    {
        IEnumerable<UrlModel> GetResults(string url);
        IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(IEnumerable<UrlModel> htmlToGetTime);
        void DownloadToDatabase(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels);
    }
}
