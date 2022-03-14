using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace TestURLS.UrlLogic
{
    public class LogicScanByHTML
    {
        private readonly UrlSettings _settingsUrl = new UrlSettings();
        private readonly HttpLogic _getHttp = new HttpLogic();

        public LogicScanByHTML(UrlSettings settingOfUrl, HttpLogic getResponse)
        {
            _settingsUrl = settingOfUrl;
            _getHttp = getResponse;
        }

        public LogicScanByHTML() { }

        public virtual List<string> GetUrlsFromScanPages(List<string> htmlScan)
        {
            //get main page to find only url from website
            var mainPartOfUrl = _settingsUrl.GetMainUrl(htmlScan.First());

            if (mainPartOfUrl != htmlScan.First() 
                && (htmlScan.First().Length - mainPartOfUrl.Length) != 1)
            {
                htmlScan.Add(mainPartOfUrl);
            }

            htmlScan = GetScannedUrls(htmlScan, mainPartOfUrl);

            return htmlScan;
        }

        protected virtual bool IsUrl(string url)
        {
            if (url.Contains(".html"))
            {
                return true;
            }
            if (url.Contains(".php"))
            {
                return true;
            }
            if (url.LastIndexOf("/") == url.Length - 1)
            {
                return true;
            }
            return false;
        }

        protected virtual List<string> GetScannedUrls(List<string> htmlScan, string mainPartOfUrl)
        {
            var scannedPages = new List<string>();
            scannedPages.AddRange(htmlScan);

            for (int i = 0; i < htmlScan.Count; i++)
            {
                //var matches = new List<string>();
                var htmlTxt = _getHttp.GetBodyFromUrl(htmlScan[i]);

                if (htmlTxt != "")
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlTxt);

                    var matches = GetLinksFromPage(htmlDoc, mainPartOfUrl);

                    matches = matches.Except(scannedPages).ToList();
                    matches.Distinct();

                    scannedPages.AddRange(matches);

                    if (matches.Count != 0)
                    {
                        htmlScan.AddRange(matches);
                    }
                }
            }

            return htmlScan;
        }

        protected virtual List<string> GetLinksFromPage(HtmlDocument htmlDoc, string mainPartOfUrl)
        {
            var matches = new List<string>();

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];

                att.Value = _settingsUrl.GetValidUrl(att.Value, mainPartOfUrl);
                //part of existing pages doesn`t interesting

                if (att.Value.Contains(mainPartOfUrl)
                    && !att.Value.Contains("#"))
                {

                    if (IsUrl(att.Value))
                    {
                        matches.Add(att.Value);
                    }
                }
            }
            return matches;
        }
    }
}