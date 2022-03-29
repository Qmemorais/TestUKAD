using Microsoft.Extensions.DependencyInjection;

namespace TestUrls.BusinessLayer.ServiceAddScoped
{
    public static class ConfigurationToBusinessServices
    {
        public static void AddServicesBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<BusinesService>();
        }
    }
}
