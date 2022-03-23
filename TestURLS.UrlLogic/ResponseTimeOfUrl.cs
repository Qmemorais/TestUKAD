using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class ResponseTimeOfUrl
    {
        public virtual IEnumerable<UrlModelWithResponse> GetLinksWithTime(List<UrlModel> linksToGetTime)
        {
            List<UrlModelWithResponse> urlWithTime = new List<UrlModelWithResponse>();

            foreach (var modelToGetTime in linksToGetTime)
            {
                //get time of request
                var timeToResponse = Stopwatch.StartNew();
                var request = (HttpWebRequest)WebRequest.Create(modelToGetTime.Link);
                var response = (HttpWebResponse)request.GetResponse();

                timeToResponse.Stop();

                var time = (int)timeToResponse.ElapsedMilliseconds;
                var model = new UrlModelWithResponse()
                {
                    Link = modelToGetTime.Link,
                    TimeOfResponse = time
                };

                urlWithTime.Add(model);
            }

            return urlWithTime;
        }
    }
}
