using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestURLS.Models;

namespace TestURLS.UrlLogic
{
    public class LogicScanBySitemap
    {
        private readonly HttpLogic _getResponse = new HttpLogic();
        private readonly UrlSettings _settingsOfUrl = new UrlSettings();

        public LogicScanBySitemap(HttpLogic getResponse, UrlSettings settingsOfUrl)
        {
            _getResponse = getResponse;
            _settingsOfUrl = settingsOfUrl;
        }

        public LogicScanBySitemap() { }

        public virtual List<UrlWithScanPage> VerifyExistStitemap(string url)
        {
            var linksFromSitemap = new List<UrlWithScanPage>();
            var mainPartOfUrl = _settingsOfUrl.GetMainUrl(url);
            //try open page/sitemap.xml
            var isSitemapExist = _getResponse.GetBodyFromUrl(mainPartOfUrl + "/sitemap.xml");

            if (!string.IsNullOrEmpty(isSitemapExist))
            {
                linksFromSitemap = ScanSitemap(mainPartOfUrl + "/sitemap.xml");
            }

            return linksFromSitemap;
        }

        private List<UrlWithScanPage> ScanSitemap(string sitemapUrl)
        {
            //create value to get xml-document and data from
            var linksFromSitemap = new List<string>();
            var xDoc = new XmlDocument();

            xDoc.Load(sitemapUrl);

            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {

                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "loc")
                    {
                        linksFromSitemap.Add(childnode.InnerText);
                    }    
                }
            }

            linksFromSitemap = linksFromSitemap.Distinct().ToList();

            var listFromSitemap = getClassFromLinks(linksFromSitemap);

            return listFromSitemap;
        }

        protected List<UrlWithScanPage> getClassFromLinks(List<string> links)
        {
            List<UrlWithScanPage> finalListOfLinks = new List<UrlWithScanPage>();

            foreach(string link in links)
            {
                finalListOfLinks.Add(new UrlWithScanPage { Link = link, FoundAt = "sitemap" });
            }

            return finalListOfLinks;
        }
    }
}
