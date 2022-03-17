using Microsoft.Extensions.DependencyInjection;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic.ServiceAddScoped
{
    public static class ConfigurationToServices
    {
        public static void AddServicesFromLogic(this IServiceCollection services)
        {
            services.AddScoped<IHttpLogic, HttpLogic>()
                    .AddScoped<ILogicScanByHtml,LogicScanByHtml>()
                    .AddScoped<ILogicScanBySitemap,LogicScanBySitemap>()
                    .AddScoped<IMainLogic, MainLogic>()
                    .AddScoped<ITimeTracker, TimeTracker>()
                    .AddScoped<IUrlSettings, UrlSettings>();
        }
    }
}
