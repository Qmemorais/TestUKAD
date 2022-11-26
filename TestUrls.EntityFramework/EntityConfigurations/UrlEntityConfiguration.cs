using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework.EntityConfigurations
{
    internal class UrlEntityConfiguration : IEntityTypeConfiguration<UrlWithResponse>
    {
        public void Configure(EntityTypeBuilder<UrlWithResponse> builder)
        {
            builder
                .HasKey(entity => entity.Id);
            builder
                .Property(entity => entity.TimeOfResponse)
                .IsRequired();
        }
    }
}
