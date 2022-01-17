﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var firstUrl = getMainURL(htmlScan[0]);
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
                            if (!existingPages || matches[k] == firstUrl + "/")
                                if (IsPageHTML(matches[k]))
                                    //if this page is text/html then add to scanList
                                    htmlScan.Add(matches[k]);
                        }
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
            int lastSymbolBefore = url.IndexOf("/", 8);

            if (lastSymbolBefore != -1)
                url = url.Substring(0, lastSymbolBefore);
            return url;
        }
    }
}
