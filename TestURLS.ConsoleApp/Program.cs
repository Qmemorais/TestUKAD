using Microsoft.Extensions.DependencyInjection;
using TestURLS.ConsoleApp.ServiceAddScoped;
using TestURLS.UrlLogic.ServiceAddScoped;

namespace TestURLS.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LogicToConsole logicToConsole;
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            logicToConsole = serviceProvider.GetService<LogicToConsole>();
            logicToConsole.Start();
        }

        private static IServiceCollection ConfigureServices()
        {
            var servicesFromConsole = new ConfigurationConsoleService();
            var servicesFromLogic = new ConfigurationToServices();
            var services = new ServiceCollection();

            services = servicesFromConsole.AddServicesFromConsole(services);
            services = servicesFromLogic.AddServicesFromLogic(services);

            return services;
        }
    }
}
