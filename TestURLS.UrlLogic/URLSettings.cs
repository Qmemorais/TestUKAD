using System.Collections.Generic;
using System.Linq;

namespace TestURLS.UrlLogic
{
    public class URLSettings
    {
        public virtual string GetMainURL(string url)
        {
            List<string> getStartIndex = url.Split("://").ToList();
            int lastSymbolBefore = getStartIndex.Last().IndexOf("/") 
                + getStartIndex.First().Length
                + "://".Length;

            if (lastSymbolBefore != -1)
                url = url[..lastSymbolBefore];
            return url;
        }

        public virtual string GetValidURL(string url, string firstUrl)
        {
            if (!url.Contains("http"))
            {
                //if we have href="/index.html"
                url = firstUrl + url;

                if (!url.Contains(firstUrl + "/"))
                {
                    url = url.Insert(firstUrl.Length, "/");
                }
            }
            return url;
        }
    }
}
