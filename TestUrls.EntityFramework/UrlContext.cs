using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestUrls.EntityFramework.Entities;
using TestUrls.EntityFramework.FluentAPI;

namespace TestUrls.EntityFramework
{
    public class UrlContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<GeneralInfoEntity> InfoEntities { get; set; }
        public DbSet<UrlEntity> UrlEntities { get; set; }

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
            modelBuilder.ApplyConfiguration(new GeneralInfoConfiguration());
            modelBuilder.ApplyConfiguration(new UrlEntityConfiguration());
        }
    }
}
