using Microsoft.Extensions.DependencyInjection;

namespace TestURLS.ConsoleApp.ServiceAddScoped
{
    public static class ConfigurationConsoleService
    {
        public static void AddServicesFromConsole(this IServiceCollection services)
        {
            services
                .AddScoped<ConsoleInOut>()
                .AddScoped<LogicToConsole>()
                .AddScoped<OutputToConsole>();
        }
    }
}
