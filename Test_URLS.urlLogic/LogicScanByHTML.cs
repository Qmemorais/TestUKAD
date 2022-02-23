﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Test_URLS.urlLogic
{
    internal class LogicScanByHTML
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
                htmlSitemap = ScanExistSitemap(url, htmlSitemap);
                //
                OutputData(htmlScan, htmlSitemap);
            }
            catch(WebException e)
            {
                //catch 403 and 404 errorsdf
                WebExceptionStatus status = e.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                    Console.WriteLine((int)httpResponse.StatusCode + " - "
                        + httpResponse.StatusCode);
                }
            }
            finally
            {
                Console.Write("Press <Enter>");
                Console.ReadLine();
            }
        }

        private List<string> ScanWebPages(List<string> htmlScan)
        {
            //get main page to find only url from website
            var firstUrl = getMainURL(htmlScan[0]);
            if (!firstUrl.Equals(htmlScan[0]) && (htmlScan[0].Length - firstUrl.Length) != 1) 
                htmlScan.Add(firstUrl);

            var scannedPages = new List<string>
            {
                htmlScan[0]
            };
            var href = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
            var reg = new Regex(href, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                            TimeSpan.FromSeconds(1));

            for(int i = 0; i < htmlScan.Count; i++)
            {
                var request = (HttpWebRequest)WebRequest.Create(htmlScan[i]);
                var response = (HttpWebResponse)request.GetResponse();
                var read = new StreamReader(response.GetResponseStream(), Encoding.Default, true, 8192);
                var HTMLtxt = read.ReadToEnd();
                response.Close();

                var match = reg.Match(HTMLtxt);
                var matches = new List<string>();

                while (match.Success)
                {   //delete part "href=''"
                    var urlFromMatch = match.Value.Substring(6);

                    if (!urlFromMatch.Contains("http"))
                    {
                        //if we have href="/index.html"
                        urlFromMatch = firstUrl + urlFromMatch;
                        if (urlFromMatch.IndexOf(firstUrl + "/") == -1)
                            urlFromMatch = urlFromMatch.Insert(firstUrl.Length, "/");
                    }

                    if (urlFromMatch.Contains(firstUrl))
                    {   //delete \" at the end of string
                        var length = urlFromMatch.LastIndexOf("\"");

                        if (length != -1)
                            urlFromMatch = urlFromMatch.Substring(0, length);

                        matches.Add(urlFromMatch);
                    }

                    match = match.NextMatch();
                }
                //scan only pages that doesn`t looking for before
                matches = matches.Except(scannedPages).ToList();
                scannedPages.AddRange(matches);

                if (matches.Count!=0)
                    for(int k = 0; k < matches.Count; k++)
                    {
                        //part of existing pages doesn`t interesting
                        if(matches[k].IndexOf("#") == -1)
                        {
                            //if difference is only http or https
                            var existingPages = htmlScan.Any(web => web.IndexOf(matches[k].Substring(5)) != -1);
                            if (!existingPages && matches[k] != firstUrl + "/")
                                if (IsPageHTML(matches[k]))
                                    //if this page is text/html then add to scanList
                                    htmlScan.Add(matches[k]);
                        }
                    }
            }
            return htmlScan;
        }

        private List<string> ScanExistSitemap(string url, List<string> htmlSitemap)
        {
            var firstUrl = getMainURL(url);
            try
            {
                //try open page/sitemap.xml
                var isSitemapExist = IsPageHTML(firstUrl + "/sitemap.xml");
                htmlSitemap = ScanSitemap(firstUrl + "/sitemap.xml");
            }
            catch
            {
                //if it doesn`t exist try to find url of sitemap
                var request = WebRequest.Create(firstUrl + "/robots.txt");
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1251));
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.IndexOf("Sitemap: ") != -1)
                    {
                        htmlSitemap = ScanSitemap(line.Substring(9));
                    }
                }
                response.Close();
            }
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
            if (htmlSitemap.Count == 0)
            {
                OutputTime(htmlScan);
                Console.WriteLine("Urls(html documents) found after crawling a website: " + htmlScan.Count);
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

                Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
                OutputList(existInSitemapNotWeb);
                Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
                OutputList(existInWebNotSitemap);
                Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML");
                OutputTime(htmlScan.Union(existInSitemapNotWeb).ToList());
                Console.WriteLine("Urls(html documents) found after crawling a website: " + htmlScan.Count);
                Console.WriteLine("Urls found in sitemap: " + htmlSitemap.Count);
            }
        }

        private string getMainURL(string url)
        {
            int lastSymbolBefore = url.IndexOf("/", 8);

            if (lastSymbolBefore != -1)
                url = url.Substring(0, lastSymbolBefore);
            return url;
        }

        private List<string> ScanSitemap(string sitemapURL)
        {
            var htmlSitemap = new List<string>();
            //create value to get xml-document and data from
            var xDoc = new XmlDocument();
            try
            {
                xDoc.Load(sitemapURL);
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                    foreach (XmlNode childnode in xnode.ChildNodes)
                        if (childnode.Name == "loc")
                                    htmlSitemap.Add(childnode.InnerText);
                htmlSitemap = htmlSitemap.Distinct().ToList();
            }
            catch
            {
            }
            return htmlSitemap;
        }

        private void OutputTime(List<string> html)
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
            Console.WriteLine(new string('_', lengthURL+14));
            Console.WriteLine("{0}{1}|{2,-12}{3}", "|", "URL".PadRight(lengthURL, ' '), "Timing (ms)", "|");
            Console.WriteLine(new string('_', lengthURL+14));
            for (int i = 0; i < urlWithTime.Count(); i++)
            {
                Console.WriteLine("{0}{1}|{2,-12}{3}", "|", ((i + 1) + ") " + urlWithTime.ElementAt(i).Key).PadRight(lengthURL, ' '), urlWithTime.ElementAt(i).Value + "ms", "|");
                Console.WriteLine(new string('_', lengthURL+14));
            }
        }

        private void OutputList(List<string> html)
        {
            var lengthURL = html.Max(x => x.Length) + 4;
            Console.WriteLine(new string('_', lengthURL + 2));
            Console.WriteLine("{0}{1}{2}", "|", "URL".PadRight(lengthURL,' '), "|");
            Console.WriteLine(new string('_', lengthURL + 2));
            for (int i = 0; i < html.Count; i++)
            {
                Console.WriteLine("{0}{1}|", "|", ((i + 1) + ") " + html[i]).PadRight(lengthURL, ' '));
                Console.WriteLine(new string('_', lengthURL + 2));
            }
        }
    }
}