using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestUrls.EntityFramework;
using TestUrls.TestResultLogic.ServiceAddScoped;
using TestURLS.UrlLogic.ServiceAddScoped;

namespace TestUrl.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();

            services.AddControllersWithViews();
            services.AddServicesFromLogic();
            services.AddServicesBusinessLayer();

            var connection = Configuration.GetConnectionString("ConnectionUrlDatabase");
            services.AddEfRepository<TestUrlsDbContext>(options => options.UseSqlServer(connection));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TestUrlsDbContext _dbContext)
        {
            _dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
