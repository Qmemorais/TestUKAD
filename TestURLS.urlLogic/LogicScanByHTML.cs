using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic
{
    public class LogicScanByHtml: ILogicScanByHtml
    {
        private readonly IUrlSettings _settingsUrl;
        private readonly IHttpLogic _getHttp;

        public LogicScanByHtml(IUrlSettings settingOfUrl, IHttpLogic getResponse)
        {
            _settingsUrl = settingOfUrl;
            _getHttp = getResponse;
        }
        public IEnumerable<UrlModel> GetUrlsFromScanPages(string url)
        {
            //get main page to find only url from website
            var domainName = _settingsUrl.GetDomainName(url);
            var linksWithScanPage = new List<UrlModel>();

            linksWithScanPage.Add(new UrlModel { Link = url, IsWeb = true });

            if (domainName != url && (url.Length - domainName.Length) != 1)
            {
                var newLink = new UrlModel { Link = url, IsWeb = true };
                linksWithScanPage.Add(newLink);
            }

            linksWithScanPage = GetScannedUrls(linksWithScanPage, domainName);

            return linksWithScanPage;
        }

        private bool IsUrl(string url)
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

        private List<UrlModel> GetScannedUrls(List<UrlModel> linksWithScanPage, string domainName)
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

                    var matches = GetLinksFromPage(htmlDoc, domainName);
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

        private List<string> GetLinksFromPage(HtmlDocument htmlDoc, string domainName)
        {
            var matches = new List<string>();

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                var attributeHref = link.Attributes["href"];
                attributeHref.Value = _settingsUrl.GetValidUrl(attributeHref.Value, domainName);

                if (attributeHref.Value.Contains(domainName))
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

        private UrlModel AddLinkToClass(string url)
        {
            return new UrlModel { Link = url, IsWeb = true };
        }

        private string RemoveSymbols(string link)
        {
            if(link.Contains("#"))
            {
                var indexOfSymbol = link.IndexOf("#");
                link = link.Substring(0, indexOfSymbol);
            }
            return link;
        }

        private List<UrlModel> GetMatchesFromScanPage(List<UrlModel> linksWithScanPage, List<string> matches)
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