using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TestURLS.UrlLogic
{
    public class LogicScanBySitemap
    {
        private readonly GetRequestFromURL _getResponse = new GetRequestFromURL();
        private readonly GetSettingFromURL _settingsOfURL = new GetSettingFromURL();

        public LogicScanBySitemap(GetRequestFromURL getResponse, GetSettingFromURL settingsOfURL)
        {
            _getResponse = getResponse;
            _settingsOfURL = settingsOfURL;
        }

        public LogicScanBySitemap() { }

        public virtual List<string> VerifyExistStitemap(string url, List<string> htmlSitemap)
        {
            var firstUrl = _settingsOfURL.GetMainURL(url);
            //try open page/sitemap.xml
            var isSitemapExist = _getResponse.GetContentType(firstUrl + "/sitemap.xml");
            if (isSitemapExist)
            {
                htmlSitemap = ScanSitemap(firstUrl + "/sitemap.xml");
            }
            else
            {
                //if it doesn`t exist try to find url of sitemap
                var urlToFindWitemap = _getResponse.GetSitemapFromURL(firstUrl + "/robots.txt");
                if (urlToFindWitemap != "")
                {
                    htmlSitemap = ScanSitemap(urlToFindWitemap);
                }
            }
            return htmlSitemap;
        }

        private List<string> ScanSitemap(string sitemapURL)
        {
            var htmlSitemap = new List<string>();
            //create value to get xml-document and data from
            var xDoc = new XmlDocument();
            xDoc.Load(sitemapURL);
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
