using System.IO;
using System.Net;
using System.Text;

namespace TestURLS.UrlLogic
{
    public class HttpLogic
    {
        public virtual HttpStatusCode GetStatusCode(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                return response.StatusCode;
            }
            catch(WebException e)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                return httpResponse.StatusCode;
            }
        }

        public virtual bool GetContentType(string url)
        {
            bool contentType = false;
            //find text/html pages
            if (GetStatusCode(url) == HttpStatusCode.OK)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                contentType = response.ContentType.Equals("text/html");
                response.Close();
            }
            return contentType;
        }

        public virtual string GetBodyFromURL(string url)
        {
            var HTMLtxt = string.Empty;
            if (GetStatusCode(url) == HttpStatusCode.OK)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var encoding = response.CharacterSet;
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                HTMLtxt = reader.ReadToEnd();
                response.Close();
            }
            return HTMLtxt;
        }

        public virtual StreamReader GetSitemapFromURL(string url)
        {
            StreamReader reader = null;
            if (GetStatusCode(url) == HttpStatusCode.OK)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var encoding = response.CharacterSet;
                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                response.Close();
                return reader;
            }
            return reader;
        }
    }
}
