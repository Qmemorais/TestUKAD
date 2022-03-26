using AutoMapper;
using TestUrls.EntityFramework.Models;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.SourceMappingProfiles
{
    public class SourceMappingUrlModelProfile : Profile
    {
        public SourceMappingUrlModelProfile()
        {
            CreateMap<UrlModel, DbUrlModel>();
        }
    }
}
