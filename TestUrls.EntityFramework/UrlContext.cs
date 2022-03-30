using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework
{
    public class UrlContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<Test> InfoEntities { get; set; }
        public DbSet<TestResult> UrlWithResponseEntities { get; set; }

        public UrlContext() { }

        public UrlContext(DbContextOptions<UrlContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder().AddJsonFile("appSettings.json");
                optionsBuilder
                    .UseSqlServer(configuration
                        .Build()
                        .GetConnectionString("ConnectionUrlDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
