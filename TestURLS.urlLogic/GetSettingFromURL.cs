using System.Net;

namespace TestURLS.UrlLogic
{
    class GetSettingFromURL
    {
        public virtual bool IsPageHTML(string url)
        {
            try
            {
                //find text/html pages
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 10000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var contentType = response.ContentType.IndexOf("text/html") != -1;
                response.Close();
                return contentType;
            }
            catch
            {
                return false;
            }
        }

        public virtual string getMainURL(string url)
        {
            int lastSymbolBefore = url.IndexOf("/", 8);

            if (lastSymbolBefore != -1)
                url = url.Substring(0, lastSymbolBefore);
            return url;
        }
    }
}
