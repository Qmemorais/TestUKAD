using System.Linq;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic
{
    public class UrlSettings : IUrlSettings
    {
        public string GetDomainName(string url)
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

        public string GetValidUrl(string url, string domainName)
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

        public string GetUrlLikeFromWeb(string url, string domainName)
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
