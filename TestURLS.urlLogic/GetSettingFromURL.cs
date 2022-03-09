namespace TestURLS.UrlLogic
{
    public class GetSettingFromURL
    {
        public virtual string GetMainURL(string url)
        {
            int lastSymbolBefore = url.IndexOf("/", 8);

            if (lastSymbolBefore != -1)
                url = url[..lastSymbolBefore];
            return url;
        }
    }
}
