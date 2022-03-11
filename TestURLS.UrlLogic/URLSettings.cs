using System.Linq;

namespace TestURLS.UrlLogic
{
    public class URLSettings
    {
        public virtual string GetMainURL(string url)
        {
            var getHttpPartFromUrl = url.Split("://").FirstOrDefault();
            var getPartAfterHttp = url.Split("://").LastOrDefault();
            int CountToSubstringToGetMainURL = getPartAfterHttp.IndexOf("/") 
                + getHttpPartFromUrl.Length
                + "://".Length;

            if (CountToSubstringToGetMainURL != -1)
            {
                url = url.Substring(url.IndexOf(url.FirstOrDefault()), CountToSubstringToGetMainURL);
            }

            return url;
        }

        public virtual string GetValidURL(string url, string mainPartOfURL)
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
