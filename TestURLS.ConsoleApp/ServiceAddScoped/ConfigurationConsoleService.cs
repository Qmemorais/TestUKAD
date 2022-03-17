using Microsoft.Extensions.DependencyInjection;
using TestURLS.ConsoleApp.Interfaces;

namespace TestURLS.ConsoleApp.ServiceAddScoped
{
    public class ConfigurationConsoleService
    {
        public ServiceCollection AddServicesFromConsole(ServiceCollection services)
        {
            services
                .AddScoped<IConsoleInOut, ConsoleInOut>()
                .AddScoped<LogicToConsole>()
                .AddScoped<OutputToConsole>();

            return services;
        }
    }
}
