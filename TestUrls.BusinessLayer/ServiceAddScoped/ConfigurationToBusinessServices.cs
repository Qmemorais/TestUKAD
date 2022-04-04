using Microsoft.Extensions.DependencyInjection;

namespace TestUrls.BusinessLogic.ServiceAddScoped
{
    public static class ConfigurationToBusinessServices
    {
        public static void AddServicesBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<TestResultService>();
        }
    }
}
