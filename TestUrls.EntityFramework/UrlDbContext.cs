using Microsoft.EntityFrameworkCore;
using TestUrls.EntityFramework.Models;

namespace TestUrls.EntityFramework
{
    public class UrlDbContext : DbContext
    {
        public DbSet<DbUrlModel> UrlModels { get; set; }
        public DbSet<DbUrlModelResponse> UrlResponseModels { get; set; }

        public UrlDbContext(DbContextOptions<UrlDbContext> options)
    : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UrlModelsdb;Trusted_Connection=True;");
        }
    }
}
