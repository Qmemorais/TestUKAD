using System.IO;
using System.Net;
using System.Text;

namespace TestURLS.UrlLogic
{
    public class HttpLogic
    {
        public virtual bool GetContentType(string url)
        {
            bool contentType = false;
            //find text/html pages
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                contentType = response.ContentType == ("text/html");
                response.Close();
                return contentType;
            }
            catch
            {
                //TODO: Add logger here
            }
            return contentType;
        }

        public virtual string GetSitemapFromURL(string url)
        {
            var HtmlTxt = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var encoding = response.CharacterSet;
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                HtmlTxt = reader.ReadToEnd();
                response.Close();
                return HtmlTxt;
            }
            catch
            {
                //TODO: Add logger here
            }
            return HtmlTxt;
        }
    }
}
