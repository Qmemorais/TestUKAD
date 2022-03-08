using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace TestURLS.UrlLogic
{
    public class MainLogic
    {
        private readonly LogicScanByHTML _scanByHTML = new LogicScanByHTML();
        private readonly LogicScanBySitemap _scanBySitemap = new LogicScanBySitemap();
        private readonly GetResponseFromURL _getResponse = new GetResponseFromURL();
        private readonly OutputList _outputList = new OutputList();

        public MainLogic(LogicScanByHTML scanByHTML, LogicScanBySitemap scanBySitemap,
            GetResponseFromURL getResponse, OutputList outputList)
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

            try
            {
                //try open url
                var response = _getResponse.GetResponse(url);
                //if OK add this url to list and work
                htmlScan.Add(url);
                //scan all exist pages on web
                htmlScan = _scanByHTML.ScanWebPages(htmlScan);
                //find sitemap and if yes: scan
                htmlSitemap = _scanBySitemap.ScanExistSitemap(url, htmlSitemap);
                //
                stringToType = _outputList.OutputTables(htmlScan, htmlSitemap);
            }
            catch (WebException e)
            {
                //catch 403 and 404 errorsdf
                WebExceptionStatus status = e.Status;

                if (status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)e.Response;
                    stringToType = 
                        (IEnumerable<string>)(IEnumerable)$"{(int)httpResponse.StatusCode} - {httpResponse.StatusCode}";
                }
            }
                return stringToType;
        }
    }
}
