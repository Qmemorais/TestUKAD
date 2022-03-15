using System.Linq;

namespace TestURLS.UrlLogic
{
    public class UrlSettings
    {
        public virtual string GetDomainName(string url)
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

        public virtual string GetValidUrl(string url, string domainName)
        {
            if (!url.Contains("http"))
            {
                url = domainName + url;
                url = AddSlashAfterMainPartIfNoExist(url, domainName);
            }

            return url;
        }

        protected virtual string AddSlashAfterMainPartIfNoExist(string url, string domainName)
        {
            if (!url.Contains(domainName + "/"))
            {
                url = url.Insert(domainName.Length, "/");
            }

            return url;
        }

        public virtual string GetUrlLikeFromWeb(string url, string domainName)
        {
            if (url.Contains(domainName))
            {
                return url;
            }
            else
            {
                url = GetUrlFromSitemapToWeb(url, domainName);
                return url;
            }
        }

        protected virtual string GetUrlFromSitemapToWeb(string url, string domainName)
        {
            var getIndexOfFirstDotFromWeb = domainName.IndexOf(".");
            var getIndexOfFirstDotFromSitemap = url.IndexOf(".");

            if (getIndexOfFirstDotFromWeb != getIndexOfFirstDotFromSitemap)
            {
                url = url.Substring(getIndexOfFirstDotFromSitemap);
                var indexOfSlash = url.IndexOf("/");
                url = url.Substring(indexOfSlash);
                url = domainName + url;
            }
            return url;
        }
    }
}
