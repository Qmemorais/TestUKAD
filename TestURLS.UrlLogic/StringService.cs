using System.Linq;

namespace TestURLS.UrlLogic
{
    public class StringService
    {
        public virtual string GetDomainName(string url)
        {
            var getHttpPartFromUrl = url.Split("://").FirstOrDefault();
            var getPartAfterHttp = url.Split("://").LastOrDefault();
            var countToSubstringToGetMainUrl = getPartAfterHttp.IndexOf("/");

            if (countToSubstringToGetMainUrl != -1)
            {
                countToSubstringToGetMainUrl+= getHttpPartFromUrl.Length
                + "://".Length;
                var indexToStartSubstringUrl = url.IndexOf(url.FirstOrDefault());
                url = url.Substring(indexToStartSubstringUrl, countToSubstringToGetMainUrl);
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

        private string AddSlashAfterMainPartIfNoExist(string url, string domainName)
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

        private string GetUrlFromSitemapToWeb(string url, string domainName)
        {
            var indexOfFirstDotFromWeb = domainName.IndexOf(".");
            var indexOfFirstDotFromSitemap = url.IndexOf(".");

            if (indexOfFirstDotFromWeb != indexOfFirstDotFromSitemap)
            {
                url = url.Substring(indexOfFirstDotFromSitemap);
                var indexOfSlash = url.IndexOf("/");
                url = url.Substring(indexOfSlash);
                url = domainName + url;
            }

            return url;
        }
    }
}
