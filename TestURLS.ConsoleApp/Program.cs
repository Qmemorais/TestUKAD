using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestUrls.BusinessLogic.ServiceAddScoped;
using TestUrls.EntityFramework;
using TestURLS.ConsoleApp.ServiceAddScoped;
using TestURLS.UrlLogic.ServiceAddScoped;

namespace TestURLS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var logicToConsole = serviceProvider.GetService<LogicToConsole>();
            logicToConsole.Start();
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            var connectionString = configuration.Build().GetConnectionString("ConnectionUrlDatabase");

            services.AddServicesFromConsole();
            services.AddServicesFromLogic();
            services.AddServicesBusinessLayer();
            services.AddEfRepository<UrlContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
