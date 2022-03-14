using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using TestURLS.Models;

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

        public virtual List<UrlWithScanPage> GetUrlsFromScanPages(string url)
        {
            //get main page to find only url from website
            var mainPartOfUrl = _settingsUrl.GetMainUrl(url);
            var linksWithScanPage = new List<UrlWithScanPage>();
            var foundAt = "web";

            linksWithScanPage.Add(new UrlWithScanPage { Link = url, FoundAt = foundAt });

            if (mainPartOfUrl != url
                && (url.Length - mainPartOfUrl.Length) != 1)
            {
                var newLink = new UrlWithScanPage { Link = mainPartOfUrl, FoundAt = foundAt };
                linksWithScanPage.Add(newLink);
            }

            linksWithScanPage = GetScannedUrls(linksWithScanPage, mainPartOfUrl);

            return linksWithScanPage;
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
            if (url.Contains(".aspx"))
            {
                return true;
            }
            if (url.LastIndexOf("/") == url.Length - 1)
            {
                return true;
            }
            return false;
        }

        protected virtual List<UrlWithScanPage> GetScannedUrls(List<UrlWithScanPage> linksWithScanPage, string mainPartOfUrl)
        {
            var scannedPages = new List<string>();
            scannedPages.AddRange(linksWithScanPage.Select(x => x.Link));

            for (int i = 0; i < linksWithScanPage.Count; i++)
            {
                var htmlTxt = _getHttp.GetBodyFromUrl(linksWithScanPage[i].Link);

                if (!string.IsNullOrEmpty(htmlTxt))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlTxt);

                    var matches = GetLinksFromPage(htmlDoc, mainPartOfUrl);

                    matches = matches.Except(scannedPages).ToList();
                    matches.Distinct();

                    scannedPages.AddRange(matches);

                    linksWithScanPage = getMatchesFromScanPage(linksWithScanPage, matches);
                }
            }

            return linksWithScanPage;
        }

        protected virtual List<string> GetLinksFromPage(HtmlDocument htmlDoc, string mainPartOfUrl)
        {
            var matches = new List<string>();

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                var att = link.Attributes["href"];

                att.Value = _settingsUrl.GetValidUrl(att.Value, mainPartOfUrl);
                //part of existing pages doesn`t interesting

                if (att.Value.Contains(mainPartOfUrl))
                {
                    var getLinkWithoutSymbols = RemoveSymbols(att.Value);

                    if (IsUrl(att.Value))
                    {
                        matches.Add(getLinkWithoutSymbols);
                    }
                }
            }
            return matches;
        }

        protected virtual UrlWithScanPage AddLinkToClass(string url)
        {
            return new UrlWithScanPage { Link = url, FoundAt = "web" };
        }

        protected virtual string RemoveSymbols(string link)
        {
            if(link.Contains("#"))
            {
                var indexOfSymbol = link.IndexOf("#");
                link = link.Substring(0, indexOfSymbol);
            }
            return link;
        }

        protected virtual List<UrlWithScanPage> getMatchesFromScanPage(List<UrlWithScanPage> linksWithScanPage, List<string> matches)
        {
            if (matches.Count != 0)
            {
                foreach (string link in matches)
                {
                    linksWithScanPage.Add(AddLinkToClass(link));
                }
            }

            return linksWithScanPage;
        }
    }
}