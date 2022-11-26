using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestUrls.EntityFramework;
using TestURLS.UrlLogic.ServiceAddScoped;

namespace TestUrls.TestResultLogic.ServiceAddScoped
{
    public static class ConfigurationToBusinessServices
    {
        public static void AddServicesBusinessLayer(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var connectionString = configuration.Build().GetConnectionString("ConnectionUrlDatabase");
            services.AddEfRepository<TestUrlsDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<TestResultService>();
            services.AddServicesFromLogic();
        }
    }
}
