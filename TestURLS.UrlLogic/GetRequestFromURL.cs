using System.IO;
using System.Net;
using System.Text;

namespace TestURLS.UrlLogic
{
    public class GetRequestFromURL
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

        public virtual HttpWebResponse GetResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            return (HttpWebResponse)request.GetResponse();
        }

        public virtual bool GetContentType(string url)
        {
            var contentType = false;
            //find text/html pages
            if (GetStatusCode(url) == HttpStatusCode.OK)
            {
                var response = GetResponse(url).ContentType;
                contentType = response.IndexOf("text/html") != -1;
            }
            return contentType;
        }

        public virtual string GetBodyFromURL(string url)
        {
            var HTMLtxt = "";
            if (GetStatusCode(url) == HttpStatusCode.OK)
            {
                var response = GetResponse(url);
                var read = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192);
                HTMLtxt = read.ReadToEnd();
                response.Close();
            }
            return HTMLtxt;
        }

        public virtual string GetSitemapFromURL(string url)
        {
            string line;
            string sitemap = "";
            if (GetStatusCode(url) == HttpStatusCode.OK)
            {
                var response = GetResponse(url);
                var reader = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192);
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.IndexOf("Sitemap: ") != -1)
                    {
                        sitemap = line[9..];
                    }
                }
                response.Close();
            }
            return sitemap;
        }
    }
}
