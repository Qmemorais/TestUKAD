using System.Collections.Generic;
using System.Linq;
using System.Xml;

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

        public virtual List<string> VerifyExistStitemap(string url, List<string> htmlSitemap)
        {
            var firstUrl = _settingsOfUrl.GetMainUrl(url);
            //try open page/sitemap.xml
            var isSitemapExist = _getResponse.GetBodyFromUrl(firstUrl + "/sitemap.xml");

            if (isSitemapExist != "")
            {
                htmlSitemap = ScanSitemap(firstUrl + "/sitemap.xml");
            }
            else
            {
                //if it doesn`t exist try to find url of sitemap
                var reader = _getResponse.GetBodyFromUrl(firstUrl + "/robots.txt");

                if (reader.Contains("Sitemap: "))
                {
                    var sitemapUrl = reader.Split("Sitemap: ").LastOrDefault();

                    htmlSitemap = ScanSitemap(sitemapUrl);
                }
            }

            return htmlSitemap;
        }

        private List<string> ScanSitemap(string sitemapUrl)
        {
            //create value to get xml-document and data from
            var htmlSitemap = new List<string>();
            var xDoc = new XmlDocument();

            xDoc.Load(sitemapUrl);

            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {

                foreach (XmlNode childnode in xnode.ChildNodes)
                {

                    if (childnode.Name == "loc")
                    {
                        htmlSitemap.Add(childnode.InnerText);
                    }    
                }
            }

            htmlSitemap = htmlSitemap.Distinct().ToList();

            return htmlSitemap;
        }
    }
}
