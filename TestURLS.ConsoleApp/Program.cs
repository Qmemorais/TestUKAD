using Microsoft.Extensions.DependencyInjection;
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

            services.AddServicesFromConsole();
            services.AddServicesFromLogic();

            return services;
        }
    }
}
