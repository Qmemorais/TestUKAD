using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework
{
    public class UrlContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<GeneralInfoEntity> InfoEntities { get; set; }
        public DbSet<UrlEntity> UrlEntities { get; set; }
        public DbSet<UrlResponseEntity> UrlResponseEntities { get; set; }

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
            // использование Fluent API
            modelBuilder.Entity<GeneralInfoEntity>()
                .Property(info => info.CreateAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<UrlEntity>()
            .HasOne(url => url.InfoEntity)
            .WithMany(info => info.UrlEntities)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UrlResponseEntity>()
            .HasOne(url => url.InfoEntity)
            .WithMany(info => info.UrlResponseEntities)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
