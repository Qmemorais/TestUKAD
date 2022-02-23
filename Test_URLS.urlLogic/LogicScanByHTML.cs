using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Test_URLS.urlLogic
{
    internal class LogicScanByHTML
    {
        public virtual List<string> ScanWebPages(List<string> htmlScan)
        {
            TypeOfURL ofURL = new TypeOfURL();
            //get main page to find only url from website
            var firstUrl = ofURL.getMainURL(htmlScan[0]);
            if (!firstUrl.Equals(htmlScan[0]) && (htmlScan[0].Length - firstUrl.Length) != 1)
                htmlScan.Add(firstUrl);

            var scannedPages = new List<string>
            {
                htmlScan[0]
            };
            var href = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))";
            var reg = new Regex(href, RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                            TimeSpan.FromSeconds(1));

            for (int i = 0; i < htmlScan.Count; i++)
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

                if (matches.Count != 0)
                    for (int k = 0; k < matches.Count; k++)
                    {
                        //part of existing pages doesn`t interesting
                        if (matches[k].IndexOf("#") == -1)
                        {
                            //if difference is only http or https
                            var existingPages = htmlScan.Any(web => web.IndexOf(matches[k].Substring(5)) != -1);
                            if (!existingPages && matches[k] != firstUrl + "/")
                                if (ofURL.IsPageHTML(matches[k]))
                                    //if this page is text/html then add to scanList
                                    htmlScan.Add(matches[k]);
                        }
                    }
            }
            return htmlScan;
        }
    }
}