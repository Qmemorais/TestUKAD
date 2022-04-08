using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework
{
    public class TestUrlsDbContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<Test> Test { get; set; }
        public DbSet<TestResult> TestResult { get; set; }

        public TestUrlsDbContext() { }

        public TestUrlsDbContext(DbContextOptions<TestUrlsDbContext> options)
            : base(options)
        {
            Database.Migrate();
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
