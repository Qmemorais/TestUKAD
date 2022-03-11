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
            var mainPartOfURL = _settingsURL.GetMainURL(htmlScan.First());

            if (mainPartOfURL != htmlScan.First() 
                && (htmlScan.First().Length - mainPartOfURL.Length) != 1)
            {
                htmlScan.Add(mainPartOfURL);
            }

            var scannedPages = new List<string>();
            scannedPages.AddRange(htmlScan);

            for (int i = 0; i < htmlScan.Count; i++)
            {
                var matches = new List<string>();
                var HTMLtxt = _getHttp.GetSitemapFromURL(htmlScan[i]);

                if (HTMLtxt != "")
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(HTMLtxt);

                    foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        HtmlAttribute att = link.Attributes["href"];

                        att.Value = _settingsURL.GetValidURL(att.Value, mainPartOfURL);
                        //part of existing pages doesn`t interesting

                        if (att.Value.Contains(mainPartOfURL)
                            && !att.Value.Contains("#"))
                        {

                            if (IsURL(att.Value))
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
                                .Any(web => web.IndexOf(matches[k].Substring("https".Length)) != -1);

                            if (!existingPages && matches[k] != mainPartOfURL + "/")
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
            }

            return htmlScan;
        }

        protected virtual bool IsURL(string url)
        {
            if (url.Contains(".html") ||
                url.Contains(".php") ||
                url.LastIndexOf("/") == url.Length - 1)
            {
                return true;
            }

            else { return false; }
        }
    }
}