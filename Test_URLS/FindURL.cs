using System;
using System.Collections.Generic;
using System.Net;

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
                response.Close();
                return (response.ContentType.IndexOf("text/html") != -1);
            }
            catch
            {
                return false;
            }
        }

        private void OutputData(List<string> htmlScan, List<string> htmlSitemap)
        {
                
        }
    }
}
