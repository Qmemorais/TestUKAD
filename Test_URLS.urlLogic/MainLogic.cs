using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Test_URLS.urlLogic
{
    public class MainLogic
    {
        public virtual List<string> GetResults(string url)
        {
            //values to work
            GetSettingFromURL ofURL = new GetSettingFromURL();
            LogicScanByHTML scanByHTML = new LogicScanByHTML();
            LogicScanBySitemap scanBySitemap = new LogicScanBySitemap();
            var htmlScan = new List<string>();
            var htmlSitemap = new List<string>();
            var stringToType = new List<string>();

            try
            {
                //try open url
                var isWebContent = ofURL.IsPageHTML(url);
                //if OK add this url to list and work
                htmlScan.Add(url);
                //scan all exist pages on web
                htmlScan = scanByHTML.ScanWebPages(htmlScan);
                //find sitemap and if yes: scan
                htmlSitemap = scanBySitemap.ScanExistSitemap(url, htmlSitemap);
                //
                stringToType = OutputData(htmlScan, htmlSitemap);
            }
            catch (WebException e)
            {
                //catch 403 and 404 errorsdf
                WebExceptionStatus status = e.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                    stringToType.Add((int)httpResponse.StatusCode + " - "
                        + httpResponse.StatusCode);
                }
            }
                return stringToType;
        }

        private List<string> OutputData(List<string> htmlScan, List<string> htmlSitemap)
        {
            var stringToType = new List<string>();

            if (htmlSitemap.Count == 0)
            {
                stringToType = OutputTime(htmlScan, stringToType);
                stringToType.Add("Urls(html documents) found after crawling a website: " + htmlScan.Count);
            }
            else
            {
                var existInSitemapNotWeb = new List<string>();
                var existInWebNotSitemap = new List<string>();
                foreach (string url in htmlScan)
                {//find any url like this. this foreach better then remove http/https and add after distinct method
                    if (!(htmlSitemap.Any(web => web.IndexOf(url.Substring(5)) != -1)))
                        existInWebNotSitemap.Add(url);
                }
                foreach (string url in htmlSitemap)
                {
                    if (!(htmlScan.Any(web => web.IndexOf(url.Substring(5)) != -1)))
                        existInSitemapNotWeb.Add(url);
                }

                stringToType.Add("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
                stringToType = OutputList(existInSitemapNotWeb, stringToType);
                stringToType.Add("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
                stringToType = OutputList(existInWebNotSitemap, stringToType);
                stringToType.Add("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");
                stringToType = OutputTime(htmlScan.Union(existInSitemapNotWeb).ToList(), stringToType);
                stringToType.Add("Urls(html documents) found after crawling a website: " + htmlScan.Count);
                stringToType.Add("Urls found in sitemap: " + htmlSitemap.Count);
            }
            return stringToType;
        }

        private List<string> OutputTime(List<string> html, List<string> stringToType)
        {
            var urlWithTime = new Dictionary<string, int>();
            foreach (string url in html)
            {
                //get time of request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                Stopwatch sw = Stopwatch.StartNew();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                sw.Stop();
                response.Close();
                var time = (int)sw.ElapsedMilliseconds;
                urlWithTime.Add(url, time);
            }
            urlWithTime = urlWithTime.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            var lengthURL = html.Max(x => x.Length) + 4;
            stringToType.Add(new string('_', lengthURL + 14));
            stringToType.Add(String.Format("{0}{1}|{2,-12}{3}", "|", "URL".PadRight(lengthURL, ' '), "Timing (ms)", "|"));
            stringToType.Add(new string('_', lengthURL + 14));
            for (int i = 0; i < urlWithTime.Count; i++)
            {
                stringToType.Add(String.Format("{0}{1}|{2,-12}{3}", "|", ((i + 1) + ") " + urlWithTime.ElementAt(i).Key).PadRight(lengthURL, ' '), urlWithTime.ElementAt(i).Value + "ms", "|"));
                stringToType.Add(new string('_', lengthURL + 14));
            }
            return stringToType;
        }

        private List<string> OutputList(List<string> html, List<string> stringToType)
        {
            var lengthURL = html.Max(x => x.Length) + 4;
            stringToType.Add(new string('_', lengthURL + 2));
            stringToType.Add(String.Format("{0}{1}{2}", "|", "URL".PadRight(lengthURL, ' '), "|"));
            stringToType.Add(new string('_', lengthURL + 2));
            for (int i = 0; i < html.Count; i++)
            {
                stringToType.Add(String.Format("{0}{1}|", "|", ((i + 1) + ") " + html[i]).PadRight(lengthURL, ' ')));
                stringToType.Add(new string('_', lengthURL + 2));
            }
            return stringToType;
        }
    }
}
