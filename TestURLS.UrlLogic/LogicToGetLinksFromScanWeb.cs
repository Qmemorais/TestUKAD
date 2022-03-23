using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic
{
    public class LogicToGetLinksFromScanWeb: ILogicToGetLinksFromScanWeb
    {
        private readonly ChangesAboveLink _settingsUrl;
        private readonly HttpLogic _getHttp;

        public LogicToGetLinksFromScanWeb(ChangesAboveLink settingOfUrl, HttpLogic getResponse)
        {
            _settingsUrl = settingOfUrl;
            _getHttp = getResponse;
        }

        public IEnumerable<string> GetUrlsFromScanPages(string url)
        {
            //get main page to find only url from website
            var domainName = _settingsUrl.GetDomainName(url);
            var linksFromScanPage = GetScannedUrls(url, domainName);

            return linksFromScanPage;
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

        private List<string> GetScannedUrls(string url, string domainName)
        {
            var scannedPages = new List<string>() 
            { 
                url 
            };
            var linksWithScanPage = new List<string>() 
            { 
                url 
            };

            for (int i = 0; i < linksWithScanPage.Count; i++)
            {
                var htmlTxt = _getHttp.GetBodyFromUrl(linksWithScanPage[i]);

                if (!string.IsNullOrEmpty(htmlTxt))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlTxt);

                    var linksThatNotScannedYet = GetLinksFromPage(htmlDoc, domainName)
                                                    .Except(scannedPages)
                                                    .Distinct()
                                                    .ToList();
                    scannedPages.AddRange(linksThatNotScannedYet);

                    linksWithScanPage = GetMatchesFromScanPage(linksWithScanPage, linksThatNotScannedYet).ToList();
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

        private string RemoveSymbols(string link)
        {
            if(link.Contains("#"))
            {
                var indexOfSymbol = link.IndexOf("#");
                link = link.Substring(0, indexOfSymbol);
            }
            return link;
        }

        private List<string> GetMatchesFromScanPage(
            List<string> linksWithScanPage, 
            List<string> linksThatNotScannedYet)
        {
            if (linksThatNotScannedYet.Count != 0)
            {
                    linksWithScanPage.AddRange(linksThatNotScannedYet);
            }

            return linksWithScanPage;
        }
    }
}