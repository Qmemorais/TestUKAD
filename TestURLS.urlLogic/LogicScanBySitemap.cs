using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestURLS.UrlLogic
{
    public class LogicScanBySitemap
    {
        private readonly GetResponseFromURL _getResponse = new GetResponseFromURL();
        private readonly GetSettingFromURL _settingsOfURL = new GetSettingFromURL();

        public LogicScanBySitemap(GetResponseFromURL getResponse, GetSettingFromURL settingsOfURL)
        {
            _getResponse = getResponse;
            _settingsOfURL = settingsOfURL;
        }

        public LogicScanBySitemap() { }

        public virtual List<string> ScanExistSitemap(string url, List<string> htmlSitemap)
        {
            var firstUrl = _settingsOfURL.GetMainURL(url);
            try
            {
                //try open page/sitemap.xml
                var isSitemapExist = _settingsOfURL.IsPageHTML(firstUrl + "/sitemap.xml");
                htmlSitemap = ScanSitemap(firstUrl + "/sitemap.xml");
            }
            catch
            {
                //if it doesn`t exist try to find url of sitemap
                var response = _getResponse.GetResponse(firstUrl + "/robots.txt");
                var reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1251));
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.IndexOf("Sitemap: ") != -1)
                    {
                        htmlSitemap = ScanSitemap(line[9..]);
                    }
                }
                response.Close();
            }
            return htmlSitemap;
        }

        private List<string> ScanSitemap(string sitemapURL)
        {
            var htmlSitemap = new List<string>();
            //create value to get xml-document and data from
            var xDoc = new XmlDocument();
            try
            {
                xDoc.Load(sitemapURL);
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                    foreach (XmlNode childnode in xnode.ChildNodes)
                        if (childnode.Name == "loc")
                            htmlSitemap.Add(childnode.InnerText);
                htmlSitemap = htmlSitemap.Distinct().ToList();
            }
            catch
            {
            }
            return htmlSitemap;
        }
    }
}
