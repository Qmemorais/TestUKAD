using System.Collections.Generic;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.Interfaces
{
    public interface ITimeTracker
    {
        IEnumerable<UrlModelWithResponse> GetLinksWithTime(List<UrlModel> linksToGetTime);
    }
}
