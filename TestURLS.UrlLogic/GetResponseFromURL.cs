using System.Net;

namespace TestURLS.UrlLogic
{
    public class GetResponseFromURL
    {
        public virtual HttpWebResponse GetResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            return (HttpWebResponse)request.GetResponse();
        }
    }
}
