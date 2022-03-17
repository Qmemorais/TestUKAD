using Microsoft.Extensions.DependencyInjection;
using TestURLS.ConsoleApp.Interfaces;

namespace TestURLS.ConsoleApp.ServiceAddScoped
{
    public static class ConfigurationConsoleService
    {
        public static void AddServicesFromConsole(this IServiceCollection services)
        {
            services
                .AddScoped<IConsoleInOut, ConsoleInOut>()
                .AddScoped<LogicToConsole>()
                .AddScoped<IOutputToConsole, OutputToConsole>();
        }
    }
}
