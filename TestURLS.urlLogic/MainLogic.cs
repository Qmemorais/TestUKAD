using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace TestURLS.UrlLogic
{
    public class MainLogic
    {
        private readonly LogicScanByHTML _scanByHTML = new LogicScanByHTML();
        private readonly LogicScanBySitemap _scanBySitemap = new LogicScanBySitemap();
        private readonly HttpLogic _getResponse = new HttpLogic();
        private List<string> htmlScanSitemap = new List<string>();
        private List<string> htmlScanWeb = new List<string>();

        public List<string> HtmlGetUrlFromSitemap { get { return htmlScanSitemap; } }
        public List<string> HtmlGetUrlFromWeb { get { return htmlScanWeb; } }

        public MainLogic(LogicScanByHTML scanByHTML, LogicScanBySitemap scanBySitemap,
            HttpLogic getResponse)
        {
            _scanByHTML = scanByHTML;
            _scanBySitemap = scanBySitemap;
            _getResponse = getResponse;
        }

        public MainLogic() { }

        public virtual void GetResults(string url)
        {
            // values to work
            var statusCode = _getResponse.GetStatusCode(url);

            if (statusCode == HttpStatusCode.OK)
            {
                // if OK add this url to list and work
                htmlScanWeb.Add(url);
                // scan all exist pages on web
                htmlScanWeb = _scanByHTML.ScanByXMLParse(htmlScanWeb);
                // find sitemap and if yes: scan
                htmlScanSitemap = _scanBySitemap.VerifyExistStitemap(url, htmlScanSitemap);
            }
        }

        public virtual List<string> GetExistLists(List<string> htmlToScan, List<string> htmlToMove)
        {
            var listToReturn = new List<string>();
            foreach (string url in htmlToScan)
            {// find any url like this. this foreach better then remove http/https
             // and add after distinct method
                if (!(htmlToMove.Any(web => web.IndexOf(url["https".Length..]) != -1)))
                    listToReturn.Add(url);
            }
            return listToReturn;
        }
    }
}
