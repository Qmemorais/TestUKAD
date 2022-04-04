using Microsoft.Extensions.DependencyInjection;

namespace TestUrls.TestResultLogic.ServiceAddScoped
{
    public static class ConfigurationToBusinessServices
    {
        public static void AddServicesBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<TestResultService>();
        }
    }
}
