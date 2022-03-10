using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace TestURLS.UrlLogic
{
    public class LogicScanBySitemap
    {
        private readonly HttpLogic _getResponse = new HttpLogic();
        private readonly URLSettings _settingsOfURL = new URLSettings();

        public LogicScanBySitemap(HttpLogic getResponse, URLSettings settingsOfURL)
        {
            _getResponse = getResponse;
            _settingsOfURL = settingsOfURL;
        }

        public LogicScanBySitemap() { }

        public virtual List<string> VerifyExistStitemap(string url, List<string> htmlSitemap)
        {
            var firstUrl = _settingsOfURL.GetMainURL(url);
            //try open page/sitemap.xml
            var isSitemapExist = _getResponse.GetStatusCode(firstUrl + "/sitemap.xml");
            if (isSitemapExist.Equals(HttpStatusCode.OK))
            {
                htmlSitemap = ScanSitemap(firstUrl + "/sitemap.xml");
            }
            else
            {
                var isRobotTxtExist = _getResponse.GetStatusCode(firstUrl + "/robots.txt");
                if (isRobotTxtExist.Equals(HttpStatusCode.OK))
                {
                    //if it doesn`t exist try to find url of sitemap
                    var reader = _getResponse.GetSitemapFromURL(firstUrl + "/robots.txt");
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("Sitemap: "))
                        {
                            var sitemapURL = line.Split("Sitemap: ").Last();

                            htmlSitemap = ScanSitemap(sitemapURL);
                        }
                    }
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
            htmlSitemap = htmlSitemap
                .Distinct()
                .ToList();
            return htmlSitemap;
        }
    }
}
