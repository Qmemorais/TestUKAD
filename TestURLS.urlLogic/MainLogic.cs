using System.Collections.Generic;
using System.Net;

namespace TestURLS.UrlLogic
{
    public class MainLogic
    {
        private readonly LogicScanByHTML _scanByHTML = new LogicScanByHTML();
        private readonly LogicScanBySitemap _scanBySitemap = new LogicScanBySitemap();
        private readonly GetRequestFromURL _getResponse = new GetRequestFromURL();
        private readonly OutputList _outputList = new OutputList();

        public MainLogic(LogicScanByHTML scanByHTML, LogicScanBySitemap scanBySitemap,
            GetRequestFromURL getResponse, OutputList outputList)
        {
            _scanByHTML = scanByHTML;
            _scanBySitemap = scanBySitemap;
            _getResponse = getResponse;
            _outputList = outputList;
        }

        public MainLogic() { }

        public virtual IEnumerable<string> GetResults(string url)
        {
            //values to work
            var htmlScan = new List<string>();
            var htmlSitemap = new List<string>();
            IEnumerable<string> stringToType = htmlScan;
            var statusCode = _getResponse.GetStatusCode(url);

            if (statusCode == HttpStatusCode.OK)
            {
                //if OK add this url to list and work
                htmlScan.Add(url);
                //scan all exist pages on web
                htmlScan = _scanByHTML.ScanWebPages(htmlScan);
                //find sitemap and if yes: scan
                htmlSitemap = _scanBySitemap.VerifyExistStitemap(url, htmlSitemap);
                //
                stringToType = _outputList.OutputTables(htmlScan, htmlSitemap);
            }
            return stringToType;
        }
    }
}
