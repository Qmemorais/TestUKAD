using System.IO;
using System.Net;
using System.Text;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic
{
    public class HttpLogic: IHttpLogic
    {
        public string GetBodyFromUrl(string url)
        {
            string bodyTxt;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();
                var encoding = response.CharacterSet;
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                bodyTxt = reader.ReadToEnd();

                response.Close();
            }
            catch (WebException)
            {
                throw;
            }

            return bodyTxt;
        }
    }
}
