namespace TestURLS.UrlLogic.Interfaces
{
    public interface IUrlSettings
    {
        string GetDomainName(string url);
        string GetValidUrl(string url, string domainName);
        string GetUrlLikeFromWeb(string url, string domainName);
    }
}
