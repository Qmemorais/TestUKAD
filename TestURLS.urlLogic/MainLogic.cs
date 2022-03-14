using System.Collections.Generic;
using System.Linq;

namespace TestURLS.UrlLogic
{
    public class MainLogic
    {
        private readonly LogicScanByHTML _scanByHTML = new LogicScanByHTML();
        private readonly LogicScanBySitemap _scanBySitemap = new LogicScanBySitemap();
        private readonly Time _getTime = new Time();

        public MainLogic(LogicScanByHTML scanByHTML, LogicScanBySitemap scanBySitemap)
        {
            _scanByHTML = scanByHTML;
            _scanBySitemap = scanBySitemap;
        }

        public MainLogic() { }

        public virtual List<List<string>> GetResults(string url)
        {
            var htmlScanSitemap = new List<string>();
            var htmlScanWeb = new List<string>();
            // if OK add this url to list and work
            htmlScanWeb.Add(url);
            // scan all exist pages on web
            htmlScanWeb = _scanByHTML.GetUrlsFromScanPages(htmlScanWeb);
            // find sitemap and if yes: scan
            htmlScanSitemap = _scanBySitemap.VerifyExistStitemap(url, htmlScanSitemap);

            var listToReturn = new List<List<string>>()
            {
                htmlScanWeb,
                htmlScanSitemap
            };

            return listToReturn;
        }

        public virtual List<string> GetExistLists(List<string> htmlToScan, List<string> htmlToMove)
        {
            var listToReturn = new List<string>();
            foreach (string url in htmlToScan)
            {// find any url like this. this foreach better then remove http/https
             // and add after distinct method
                if (!(htmlToMove.Any(web => web.IndexOf(url.Substring("https".Length)) != -1)))
                {
                    listToReturn.Add(url);
                }
            }
            return listToReturn;
        }

        public virtual List<UrlTimeModel> GetUrlsWithTimeResponse(List<string> html)
        {
            var values = _getTime.GetLinksWithTime(html);

            return values;
        }
    }
}
