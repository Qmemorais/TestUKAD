namespace TestURLS.UrlLogic
{
    public class GetSettingFromURL
    {
        private readonly GetResponseFromURL _getResponse = new GetResponseFromURL();

        public GetSettingFromURL(GetResponseFromURL _response)
        {
            _getResponse = _response;
        }

        public GetSettingFromURL() { }

        public virtual bool IsPageHTML(string url)
        {
            try
            {
                //find text/html pages
                var response = _getResponse.GetResponse(url);
                var contentType = response.ContentType.IndexOf("text/html") != -1;
                response.Close();
                return contentType;
            }
            catch
            {
                return false;
            }
        }

        public virtual string GetMainURL(string url)
        {
            int lastSymbolBefore = url.IndexOf("/", 8);

            if (lastSymbolBefore != -1)
                url = url[..lastSymbolBefore];
            return url;
        }
    }
}
