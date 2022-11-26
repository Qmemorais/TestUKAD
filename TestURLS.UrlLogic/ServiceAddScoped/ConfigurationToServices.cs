﻿using Microsoft.Extensions.DependencyInjection;

namespace TestURLS.UrlLogic.ServiceAddScoped
{
    public static class ConfigurationToServices
    {
        public static void AddServicesFromLogic(this IServiceCollection services)
        {
            services.AddScoped<HttpService>()
                    .AddScoped<WebService>()
                    .AddScoped<SitemapService>()
                    .AddScoped<MainService>()
                    .AddScoped<ResponseService>()
                    .AddScoped<StringService>();
        }
    }
}
