using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Test_URLS
{
    internal class FindURL
    {
        public void GetContent(string url)
        {
            //values to work
            var htmlScan = new List<string>();
            var htmlSitemap = new List<string>();

            try
            {
                //try open url
                var isWebContent = IsPageHTML(url);
                //if OK add this url to list and work
                htmlScan.Add(url);
                //scan all exist pages on web
                htmlScan = ScanWebPages(htmlScan);
                //find sitemap and if yes: scan
                htmlSitemap = ScanExistSitemap(htmlSitemap);
            }
            catch(WebException e)
            {
                //catch 403 and 404 error
                WebExceptionStatus status = e.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                }
            }
            finally
            {
                OutputData(htmlScan, htmlSitemap);
                Console.Write("Press <Enter>");
                Console.ReadLine();
            }
        }

        private List<string> ScanWebPages(List<string> htmlScan)
        {
            //get main page to find only url from website
            string firstUrl = getMainURL(htmlScan[0]);
            List<string> AllPages = new List<string>();
            AllPages.Add(htmlScan[0]);
            string Href = string.Format(@"href\s*=\s*(?:[""'](?<1>[^""']*){0}[""']|(?<1>\S+))", firstUrl);
            Regex r = new Regex(Href, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                            TimeSpan.FromSeconds(1));

            for(int i = 0; i < htmlScan.Count; i++)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(htmlScan[i]);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader read = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192);
                string HTMLtxt = read.ReadToEnd();
                response.Close();
                Match match = r.Match(HTMLtxt);
                List<string> matches = new List<string>();
                while (match.Success)
                {
                    matches.Add(match.Value);
                }
            }

            return htmlScan;
        }

        private List<string> ScanExistSitemap(List<string> htmlSitemap)
        {
            return htmlSitemap;
        }

        private bool IsPageHTML(string url)
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

        private void OutputData(List<string> htmlScan, List<string> htmlSitemap)
        {
                
        }

        private string getMainURL(string url)
        {
            int firstSymbolAfter = url.IndexOf("//")+2;
            int lastSymbolBefore = url.IndexOf("/", 8);
            if (lastSymbolBefore != -1)
                url = url.Substring(firstSymbolAfter, lastSymbolBefore - firstSymbolAfter);
            return url;
        }
    }
}
