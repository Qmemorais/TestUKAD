using Microsoft.Extensions.DependencyInjection;
using TestUrls.TestResultLogic.ServiceAddScoped;
using TestURLS.ConsoleApp.ServiceAddScoped;

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

            services.AddServicesFromConsole();
            services.AddServicesBusinessLayer();

            return services;
        }
    }
}
