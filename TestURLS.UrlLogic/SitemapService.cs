using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TestURLS.UrlLogic
{
    public class SitemapService
    {
        private readonly HttpService _httpService;
        private readonly StringService _stringService;

        public SitemapService(HttpService httpService, StringService stringService)
        {
            _httpService = httpService;
            _stringService = stringService;
        }

        public virtual IEnumerable<string> GetLinksFromSitemapIfExist(string url)
        {
            var linksFromSitemap = new List<string>();
            var domainName = _stringService.GetDomainName(url);
            //try open page/sitemap.xml
            var getBodyFromSitemapIfExist = _httpService.GetBodyFromUrl(domainName + "/sitemap.xml");

            if (!string.IsNullOrEmpty(getBodyFromSitemapIfExist))
            {
                linksFromSitemap = ScanSitemap(getBodyFromSitemapIfExist);
            }

            return linksFromSitemap;
        }

        private List<string> ScanSitemap(string sitemapUrl)
        {
            //create value to get xml-document and data from
            var linksFromSitemap = new List<string>();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sitemapUrl);

            var xmlElement = xmlDoc.DocumentElement;

            foreach (XmlNode xmlNode in xmlElement)
            {
                foreach (XmlNode childnode in xmlNode.ChildNodes)
                {
                    if (childnode.Name == "loc")
                    {
                        linksFromSitemap.Add(childnode.InnerText);
                    }    
                }
            }

            linksFromSitemap = linksFromSitemap
                .Distinct()
                .ToList();

            return linksFromSitemap;
        }
    }
}
