using System.Linq;

namespace TestURLS.UrlLogic
{
    public class UrlSettings
    {
        public virtual string GetMainUrl(string url)
        {
            var getHttpPartFromUrl = url.Split("://").FirstOrDefault();
            var getPartAfterHttp = url.Split("://").LastOrDefault();
            var countToSubstringToGetMainURL = getPartAfterHttp.IndexOf("/") 
                + getHttpPartFromUrl.Length
                + "://".Length;

            if (countToSubstringToGetMainURL != -1)
            {
                var indexToStartSubstringUrl = url.IndexOf(url.FirstOrDefault());
                url = url.Substring(indexToStartSubstringUrl, countToSubstringToGetMainURL);
            }

            return url;
        }

        public virtual string GetValidUrl(string url, string mainPartOfURL)
        {
            if (!url.Contains("http"))
            {
                url = mainPartOfURL + url;
                url = AddSlashAfterMainPartIfNoExist(url, mainPartOfURL);
            }

            return url;
        }

        protected virtual string AddSlashAfterMainPartIfNoExist(string url, string mainPartOfURL)
        {
            if (!url.Contains(mainPartOfURL + "/"))
            {
                url = url.Insert(mainPartOfURL.Length, "/");
            }

            return url;
        }
    }
}
