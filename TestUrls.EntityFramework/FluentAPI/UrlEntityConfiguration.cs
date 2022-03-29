using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework.FluentAPI
{
    internal class UrlEntityConfiguration : IEntityTypeConfiguration<UrlEntity>
    {
        public void Configure(EntityTypeBuilder<UrlEntity> builder)
        {
            builder
                .HasKey(entity => entity.Id);
            builder
                .Property(entity => entity.TimeOfResponse)
                .IsRequired(true);
            builder
                .HasOne(url => url.InfoEntity)
                .WithMany(info => info.UrlEntities)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
