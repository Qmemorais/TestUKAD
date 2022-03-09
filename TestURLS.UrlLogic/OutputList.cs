using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TestURLS.UrlLogic
{
    public class OutputList
    {
        private readonly GetRequestFromURL _getResponse = new GetRequestFromURL();

        public OutputList(GetRequestFromURL getResponse)
        {
            _getResponse = getResponse;
        }

        public OutputList() { }

        public virtual List<string> OutputTables(List<string> htmlScan, List<string> htmlSitemap)
        {
            var stringToType = new List<string>();

            if (htmlSitemap.Count == 0)
            {
                stringToType = OutputTime(htmlScan, stringToType);
                stringToType.Add("Urls(html documents) found after crawling a website: " + htmlScan.Count);
            }
            else
            {
                var existInSitemapNotWeb = GetExistLists(htmlSitemap, htmlScan);
                var existInWebNotSitemap = GetExistLists(htmlScan, htmlSitemap);

                stringToType.Add("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
                stringToType = OutputURLS(existInSitemapNotWeb, stringToType);
                stringToType.Add("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
                stringToType = OutputURLS(existInWebNotSitemap, stringToType);
                stringToType.Add("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");
                stringToType = OutputTime(htmlScan.Union(existInSitemapNotWeb).ToList(), stringToType);
                stringToType.Add("Urls(html documents) found after crawling a website: " + htmlScan.Count);
                stringToType.Add("Urls found in sitemap: " + htmlSitemap.Count);
            }
            return stringToType;
        }

        protected virtual List<string> OutputTime(List<string> html, List<string> stringToType)
        {
            var urlWithTime = new Dictionary<string, int>();

            foreach (string url in html)
            {
                //get time of request
                Stopwatch sw = Stopwatch.StartNew();
                var response = _getResponse.GetStatusCode(url);
                sw.Stop();
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

        protected virtual List<string> OutputURLS(List<string> html, List<string> stringToType)
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

        protected virtual List<string> GetExistLists(List<string> htmlToScan, List<string> htmlToMove)
        {
            var listToReturn = new List<string>();
            foreach (string url in htmlToScan)
            {//find any url like this. this foreach better then remove http/https and add after distinct method
                if (!(htmlToMove.Any(web => web.IndexOf(url[5..]) != -1)))
                    listToReturn.Add(url);
            }
            return listToReturn;
        }

    }
}
