using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestUrls.EntityFramework.Models;
using TestUrls.EntityFramework.UnitOfWorkPatern;
using TestURLS.UrlLogic.Interfaces;
using TestURLS.UrlLogic.Models;
using TestURLS.UrlLogic.SourceMappingProfiles;

namespace TestURLS.UrlLogic
{
    public class MainLogic : IMainLogic
    {
        private readonly ILogicToGetLinksFromScanWeb _getLinksFromScanWeb;
        private readonly ILogicToGetLinksFromSitemap _getLinksFromScanSitemap;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ChangesAboveLink _getChanges;
        private readonly ResponseTimeOfUrl _getResponseTime;

        public MainLogic(
            ILogicToGetLinksFromScanWeb scanByHtml, 
            ILogicToGetLinksFromSitemap scanBySitemap,
            IUnitOfWork unitOfWork,
            ChangesAboveLink settings,
            ResponseTimeOfUrl timeTracker)
        {
            _getLinksFromScanWeb = scanByHtml;
            _getLinksFromScanSitemap = scanBySitemap;
            _unitOfWork = unitOfWork;
            _getChanges = settings;
            _getResponseTime = timeTracker;
        }
        
        public IEnumerable<UrlModel> GetResults(string url)
        {
            // scan all exist pages on web
            var linksFromScanPages = _getLinksFromScanWeb.GetUrlsFromScanPages(url);
            // find sitemap and if yes: scan
            var linksFromScanSitemap = _getLinksFromScanSitemap.GetLinksFromSitemapIfExist(url);
            var allUrls = GetLinksWithUrlModel(linksFromScanPages.ToList(), linksFromScanSitemap);

            return allUrls;
        }

        public IEnumerable<UrlModelWithResponse> GetUrlsWithTimeResponse(IEnumerable<UrlModel> htmlToGetTime)
        {
            var values = _getResponseTime.GetLinksWithTime(htmlToGetTime);

            return values;
        }

        private List<UrlModel> GetLinksWithUrlModel(
            List<string> linksFromScanPages,
            IEnumerable<string> linksFromScanSitemap)
        {
            var allLinks = new List<UrlModel>();
            var linksOnlyFromScanPages = linksFromScanPages.Except(linksFromScanSitemap);

            var modelsFromPages = MakeUrlModelFromScanPages(linksOnlyFromScanPages);
            var modelsFromSitemap = MakeUrlModelFromSitemap(linksFromScanPages, linksFromScanSitemap);
            allLinks.AddRange(modelsFromPages);
            allLinks.AddRange(modelsFromSitemap);

            return allLinks;
        }

        private List<UrlModel> MakeUrlModelFromScanPages(IEnumerable<string> linksFromScanPages)
        {
            var urlModelOfPages = new List<UrlModel>();

            foreach (var linkFromPage in linksFromScanPages)
            {
                urlModelOfPages.Add(new UrlModel { Link = linkFromPage, IsWeb = true });
            }

            return urlModelOfPages;
        }

        private List<UrlModel> MakeUrlModelFromSitemap(List<string> linksFromScanPages,
            IEnumerable<string> linksFromScanSitemap)
        {
            var urlModelOfSitemap = new List<UrlModel>();
            var firstLink = linksFromScanPages.FirstOrDefault();
            var domainName = _getChanges.GetDomainName(firstLink);

            foreach (var linkFromList in linksFromScanSitemap)
            {
                var newLinkFromSitemap = _getChanges.GetUrlLikeFromWeb(linkFromList, domainName);
                var indexLinkFromList = linksFromScanPages.FindIndex(link => string.Equals(link, newLinkFromSitemap));

                if (indexLinkFromList > -1)
                {
                    urlModelOfSitemap.Add(new UrlModel { Link = newLinkFromSitemap, IsSitemap = true, IsWeb = true });
                }
                else
                {
                    urlModelOfSitemap.Add(new UrlModel { Link = newLinkFromSitemap, IsSitemap = true });
                }
            }

            return urlModelOfSitemap;
        }

        public void DownloadToDatabase(IEnumerable<UrlModel> urlModels, IEnumerable<UrlModelWithResponse> urlResponseModels)
        {
            var mapper = GetConfigurationForMapper();
            var mappedUrlModel = mapper
            .Map<IEnumerable<UrlModel>, IEnumerable<DbUrlModel>>(urlModels);
            var mappedUrlModelResponse = mapper
                .Map<IEnumerable<UrlModelWithResponse>, IEnumerable<DbUrlModelResponse>>(urlResponseModels);

            _unitOfWork.UrlModels.AddRange(mappedUrlModel);
            _unitOfWork.Save();
            _unitOfWork.UrlResponseModels.AddRange(mappedUrlModelResponse);
            _unitOfWork.Save();
        }

        private Mapper GetConfigurationForMapper()
        {
            var config = new MapperConfiguration(
                config =>
                {
                    config.AddProfile<SourceMappingUrlModelProfile>();
                    config.AddProfile<SourceMappingUrlResponseModelProfile>();
                });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
