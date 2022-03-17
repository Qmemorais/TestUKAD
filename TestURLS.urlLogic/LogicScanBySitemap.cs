﻿using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic
{
    public class LogicScanBySitemap: ILogicScanBySitemap
    {
        private readonly HttpLogic _getResponse;
        private readonly UrlSettings _settingsOfUrl;

        public LogicScanBySitemap(HttpLogic getResponse, UrlSettings settingsOfUrl)
        {
            _getResponse = getResponse;
            _settingsOfUrl = settingsOfUrl;
        }

        public IEnumerable<string> GetLinksFromSitemapIfExist(string url)
        {
            var linksFromSitemap = new List<string>();
            var domainName = _settingsOfUrl.GetDomainName(url);
            //try open page/sitemap.xml
            var isSitemapExist = _getResponse.GetBodyFromUrl(domainName + "/sitemap.xml");

            if (!string.IsNullOrEmpty(isSitemapExist))
            {
                linksFromSitemap = ScanSitemap(isSitemapExist);
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
