using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework.EntityConfigurations
{
    public class SiteTestEntityConfiguration : IEntityTypeConfiguration<SiteTestEntity>
    {
        public void Configure(EntityTypeBuilder<SiteTestEntity> builder)
        {
            builder
                .HasKey(info => info.Id);
            builder
                .Property(info => info.CreateAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
