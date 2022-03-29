using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework.FluentAPI
{
    public class GeneralInfoConfiguration : IEntityTypeConfiguration<GeneralInfoEntity>
    {
        public void Configure(EntityTypeBuilder<GeneralInfoEntity> builder)
        {
            builder
                .HasKey(info => info.Id);
            builder
                .Property(info => info.UrlWithResponseEntities)
                .IsRequired(true);
            builder
                .Property(info => info.CreateAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
