using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace TestURLS.UrlLogic
{
    public class LogicScanByHTML
    {
        private readonly URLSettings _settingsURL = new URLSettings();
        private readonly HttpLogic _getHttp = new HttpLogic();

        public LogicScanByHTML(URLSettings settingOfURL, HttpLogic getResponse)
        {
            _settingsURL = settingOfURL;
            _getHttp = getResponse;
        }

        public LogicScanByHTML() { }

        public virtual List<string> ScanByXMLParse(List<string> htmlScan)
        {
            //get main page to find only url from website
            var firstUrl = _settingsURL.GetMainURL(htmlScan.First());

            if (!firstUrl.Equals(htmlScan.First()) 
                && (htmlScan.First().Length - firstUrl.Length) != 1)
            {
                htmlScan.Add(firstUrl);
            }

            var scannedPages = new List<string>();
            scannedPages.AddRange(htmlScan);

            for (int i = 0; i < htmlScan.Count; i++)
            {
                var matches = new List<string>();
                var HTMLtxt = _getHttp.GetBodyFromURL(htmlScan[i]);

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(HTMLtxt);

                foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    HtmlAttribute att = link.Attributes["href"];

                    att.Value = _settingsURL.GetValidURL(att.Value, firstUrl);
                    //part of existing pages doesn`t interesting
                    if (att.Value.Contains(firstUrl)
                        && !att.Value.Contains("#"))
                    {   //delete \" at the end of string
                        if (att.Value.Contains(".html") ||
                            att.Value.Contains(".php") ||
                            att.Value.LastIndexOf("/") == att.Value.Length - 1)
                        {
                            matches.Add(att.Value);
                        }
                    }
                }

                matches = matches.Except(scannedPages).ToList();
                scannedPages.AddRange(matches);

                if (matches.Count != 0)
                {
                    for (int k = 0; k < matches.Count; k++)
                    {
                        //if difference is only http or https
                        var existingPages = htmlScan
                            .Any(web => web.IndexOf(matches[k][5..]) != -1);

                        if (!existingPages && matches[k] != firstUrl + "/")
                        {
                            if (_getHttp.GetContentType(matches[k]))
                            //if this page is text/html then add to scanList
                            {
                                htmlScan.Add(matches[k]);
                            }
                        }
                    }
                }
            }

            return htmlScan;
        }
    }
}