using System.IO;
using System.Net;
using System.Text;

namespace TestURLS.UrlLogic
{
    public class HttpLogic
    {

        public virtual string GetBodyFromUrl(string url)
        {
            var bodyTxt = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var encoding = response.CharacterSet;
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                bodyTxt = reader.ReadToEnd();

                response.Close();
            }
            catch(WebException ex)
            {
                //TODO: Add logger here
            }
            return bodyTxt;
        }
    }
}
