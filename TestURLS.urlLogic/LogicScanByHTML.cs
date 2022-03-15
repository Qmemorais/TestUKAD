using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class LogicScanByHtml
    {
        private readonly UrlSettings _settingsUrl = new UrlSettings();
        private readonly HttpLogic _getHttp = new HttpLogic();

        public LogicScanByHtml(UrlSettings settingOfUrl, HttpLogic getResponse)
        {
            _settingsUrl = settingOfUrl;
            _getHttp = getResponse;
        }

        public LogicScanByHtml() { }

        public virtual IEnumerable<UrlModel> GetUrlsFromScanPages(string url)
        {
            //get main page to find only url from website
            var domenName = _settingsUrl.GetDomenName(url);
            var linksWithScanPage = new List<UrlModel>();
            linksWithScanPage.Add(new UrlModel { Link = url, IsSitemap = false, IsWeb = true }); ;

            if (domenName != url && (url.Length - domenName.Length) != 1)
            {
                var newLink = new UrlModel { Link = url, IsSitemap = false, IsWeb = true };
                linksWithScanPage.Add(newLink);
            }

            linksWithScanPage = GetScannedUrls(linksWithScanPage, domenName);

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
            if (url[url.Length - 1] == '/')
            {
                return true;
            }
            return false;
        }

        protected virtual List<UrlModel> GetScannedUrls(List<UrlModel> linksWithScanPage, string domenName)
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

                    var matches = GetLinksFromPage(htmlDoc, domenName);
                    //part of existing pages doesn`t interesting
                    matches = matches
                        .Except(scannedPages)
                        .Distinct()
                        .ToList();

                    scannedPages.AddRange(matches);

                    linksWithScanPage = GetMatchesFromScanPage(linksWithScanPage, matches);
                }
            }

            return linksWithScanPage;
        }

        protected virtual List<string> GetLinksFromPage(HtmlDocument htmlDoc, string domenName)
        {
            var matches = new List<string>();

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                var attributeHref = link.Attributes["href"];
                attributeHref.Value = _settingsUrl.GetValidUrl(attributeHref.Value, domenName);

                if (attributeHref.Value.Contains(domenName))
                {
                    var getLinkWithoutSymbols = RemoveSymbols(attributeHref.Value);

                    if (IsUrl(getLinkWithoutSymbols))
                    {
                        matches.Add(getLinkWithoutSymbols);
                    }
                }
            }
            return matches;
        }

        protected virtual UrlModel AddLinkToClass(string url)
        {
            return new UrlModel { Link = url, IsSitemap = false, IsWeb = true };
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

        protected virtual List<UrlModel> GetMatchesFromScanPage(List<UrlModel> linksWithScanPage, List<string> matches)
        {
            if (matches.Count != 0)
            {
                foreach (string link in matches)
                {
                    var newLink = AddLinkToClass(link);
                    linksWithScanPage.Add(newLink);
                }
            }

            return linksWithScanPage;
        }
    }
}