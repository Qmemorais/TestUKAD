using Microsoft.Extensions.DependencyInjection;
using TestURLS.UrlLogic.Interfaces;

namespace TestURLS.UrlLogic.ServiceAddScoped
{
    public class ConfigurationToServices
    {
        public ServiceCollection AddServicesFromLogic(ServiceCollection services)
        {
            services.AddScoped<IHttpLogic, HttpLogic>()
                    .AddScoped<LogicScanByHtml>()
                    .AddScoped<LogicScanBySitemap>()
                    .AddScoped<MainLogic>()
                    .AddScoped<ITimeTracker, TimeTracker>()
                    .AddScoped<IUrlSettings, UrlSettings>();

            return services;
        }
    }
}
