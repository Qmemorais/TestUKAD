using Microsoft.Extensions.DependencyInjection;
using TestURLS.UrlLogic.ServiceAddScoped;

namespace TestUrls.TestResultLogic.ServiceAddScoped
{
    public static class ConfigurationToBusinessServices
    {
        public static void AddServicesBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<TestResultService>();
            services.AddServicesFromLogic();
        }
    }
}
