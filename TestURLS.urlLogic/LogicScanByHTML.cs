using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestURLS.UrlLogic
{
    public class LogicScanByHTML
    {
        private readonly GetSettingFromURL _settingOfURL = new GetSettingFromURL();
        private readonly GetRequestFromURL _getResponse = new GetRequestFromURL();

        public LogicScanByHTML(GetSettingFromURL settingOfURL, GetRequestFromURL getResponse)
        {
            _settingOfURL = settingOfURL;
            _getResponse = getResponse;
        }

        public LogicScanByHTML() { }

        public virtual List<string> ScanWebPages(List<string> htmlScan)
        {
            //get main page to find only url from website
            var firstUrl = _settingOfURL.GetMainURL(htmlScan[0]);

            if (!firstUrl.Equals(htmlScan[0]) && (htmlScan[0].Length - firstUrl.Length) != 1)
            {
                htmlScan.Add(firstUrl);
            }

            var scannedPages = new List<string>
            {
                htmlScan[0]
            };
            var href = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
            var reg = new Regex(href, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                            TimeSpan.FromSeconds(1));

            for (int i = 0; i < htmlScan.Count; i++)
            {
                var HTMLtxt = _getResponse.GetBodyFromURL(htmlScan[i]);

                var match = reg.Match(HTMLtxt);
                var matches = new List<string>();

                while (match.Success)
                {   //delete part "href=''"
                    var urlFromMatch = match.Value[6..];

                    if (!urlFromMatch.Contains("http"))
                    {
                        //if we have href="/index.html"
                        urlFromMatch = firstUrl + urlFromMatch;

                        if (urlFromMatch.IndexOf(firstUrl + "/") == -1)
                        {
                            urlFromMatch = urlFromMatch.Insert(firstUrl.Length, "/");
                        }
                    }

                    if (urlFromMatch.Contains(firstUrl))
                    {   //delete \" at the end of string
                        var length = urlFromMatch.LastIndexOf("\"");

                        if (length != -1)
                        {
                            urlFromMatch = urlFromMatch[..length];
                        }

                        if (urlFromMatch.Contains(".html") ||
                            urlFromMatch.Contains(".php") ||
                            urlFromMatch.LastIndexOf("/") == urlFromMatch.Length - 1)
                        {
                            matches.Add(urlFromMatch);
                        }
                    }

                    match = match.NextMatch();
                }

                //scan only pages that doesn`t looking for before
                matches = matches.Except(scannedPages).ToList();
                scannedPages.AddRange(matches);

                if (matches.Count != 0)
                {
                    for (int k = 0; k < matches.Count; k++)
                    {
                        //part of existing pages doesn`t interesting
                        if (matches[k].IndexOf("#") == -1)
                        {
                            //if difference is only http or https
                            var existingPages = htmlScan.Any(web => web.IndexOf(matches[k][5..]) != -1);

                            if (!existingPages && matches[k] != firstUrl + "/")
                            {
                                if (_getResponse.GetContentType(matches[k]))
                                //if this page is text/html then add to scanList
                                {
                                    htmlScan.Add(matches[k]);
                                }
                            }
                        }
                    }
                }
            }

            return htmlScan;
        }
    }
}