using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace Test_URLS.urlLogic
{
    class LogicScanBySitemap
    {
        public virtual List<string> ScanExistSitemap(string url, List<string> htmlSitemap)
        {
            GetSettingFromURL ofURL = new GetSettingFromURL();
            var firstUrl = ofURL.getMainURL(url);
            try
            {
                //try open page/sitemap.xml
                var isSitemapExist = ofURL.IsPageHTML(firstUrl + "/sitemap.xml");
                htmlSitemap = ScanSitemap(firstUrl + "/sitemap.xml");
            }
            catch
            {
                //if it doesn`t exist try to find url of sitemap
                var request = WebRequest.Create(firstUrl + "/robots.txt");
                var response = request.GetResponse();
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
