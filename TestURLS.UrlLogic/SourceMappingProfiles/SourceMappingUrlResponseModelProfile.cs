using AutoMapper;
using TestUrls.EntityFramework.Models;
using TestURLS.UrlLogic.Models;

namespace TestURLS.UrlLogic.SourceMappingProfiles
{
    public class SourceMappingUrlResponseModelProfile : Profile
    {
        public SourceMappingUrlResponseModelProfile()
        {
            CreateMap<UrlModelWithResponse, DbUrlModelResponse>();
        }
    }
}
