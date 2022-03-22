using Microsoft.Extensions.DependencyInjection;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic.ServiceAddScoped
{
    public static class ConfigurationToServices
    {
        public static void AddServicesFromLogic(this IServiceCollection services)
        {
            services.AddScoped<HttpLogic>()
                    .AddScoped<ILogicToGetLinksFromScanWeb,LogicToGetLinksFromScanWeb>()
                    .AddScoped<ILogicToGetLinksFromSitemap,LogicToGetLinksFromSitemap>()
                    .AddScoped<IMainLogic, MainLogic>()
                    .AddScoped<ResponseTimeOfUrl>()
                    .AddScoped<ChangesAboveLink>();
        }
    }
}
