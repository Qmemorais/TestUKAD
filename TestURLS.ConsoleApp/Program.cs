using Microsoft.Extensions.DependencyInjection;
using TestURLS.ConsoleApp.Interfaces;
using TestURLS.UrlLogic;
using TestURLS.UrlLogic.Interfaces;

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
            services
                    .AddScoped<IConsoleInOut, ConsoleInOut>()
                    .AddScoped<LogicToConsole>()
                    .AddScoped<IOutputToConsole, OutputToConsole>()

                    .AddScoped<IHttpLogic, HttpLogic>()
                    .AddScoped<ILogicScanByHtml, LogicScanByHtml>()
                    .AddScoped<ILogicScanBySitemap, LogicScanBySitemap>()
                    .AddScoped<IMainLogic, MainLogic>()
                    .AddScoped<ITimeTracker, TimeTracker>()
                    .AddScoped<IUrlSettings, UrlSettings>();

            return services;
        }
    }
}
