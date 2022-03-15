using System.Linq;

namespace TestURLS.UrlLogic
{
    public class UrlSettings
    {
        public virtual string GetDomenName(string url)
        {
            var getHttpPartFromUrl = url.Split("://").FirstOrDefault();
            var getPartAfterHttp = url.Split("://").LastOrDefault();
            var countToSubstringToGetMainURL = getPartAfterHttp.IndexOf("/");

            if (countToSubstringToGetMainURL != -1)
            {
                countToSubstringToGetMainURL+= getHttpPartFromUrl.Length
                + "://".Length;
                var indexToStartSubstringUrl = url.IndexOf(url.FirstOrDefault());
                url = url.Substring(indexToStartSubstringUrl, countToSubstringToGetMainURL);
            }

            return url;
        }

        public virtual string GetValidUrl(string url, string domenName)
        {
            if (!url.Contains("http"))
            {
                url = domenName + url;
                url = AddSlashAfterMainPartIfNoExist(url, domenName);
            }

            return url;
        }

        protected virtual string AddSlashAfterMainPartIfNoExist(string url, string domenName)
        {
            if (!url.Contains(domenName + "/"))
            {
                url = url.Insert(domenName.Length, "/");
            }

            return url;
        }

        public virtual string GetUrlLikeFromWeb(string url, string domenName)
        {
            if (url.Contains(domenName))
            {
                return url;
            }
            else
            {
                url = GetUrlFromSitemapToWeb(url, domenName);
                return url;
            }
        }

        protected virtual string GetUrlFromSitemapToWeb(string url, string domenName)
        {
            var getIndexOfFirstDotFromWeb = domenName.IndexOf(".");
            var getIndexOfFirstDotFromSitemap = url.IndexOf(".");

            if (getIndexOfFirstDotFromWeb != getIndexOfFirstDotFromSitemap)
            {
                url = url.Substring(getIndexOfFirstDotFromSitemap);
                var indexOfSlash = url.IndexOf("/");
                url = url.Substring(indexOfSlash);
                url = domenName + url;
            }
            return url;
        }
    }
}
