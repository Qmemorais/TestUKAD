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
            var services = new ServiceCollection();

            services = (ServiceCollection)ConfigurationConsoleService.AddServicesFromConsole(services);
            services = (ServiceCollection)ConfigurationToServices.AddServicesFromLogic(services);

            return services;
        }
    }
}
