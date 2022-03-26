using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestUrls.EntityFramework.Models;
using TestUrls.EntityFramework.Repository;
using TestUrls.EntityFramework.UnitOfWorkPatern;

namespace TestUrls.EntityFramework.ServiceAddScoped
{
    public static class ConfigurationEntityFrameworkService
    {
        public static void AddServicesFromEF(this IServiceCollection services)
        {
            services.AddDbContext<UrlDbContext>(options =>
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UrlModelsdb;Trusted_Connection=True;"));

            services
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUrlRepository<DbUrlModel>, UrlRepository<DbUrlModel>>()
                .AddScoped<IUrlRepository<DbUrlModelResponse>, UrlRepository<DbUrlModelResponse>>();
        }
    }
}
