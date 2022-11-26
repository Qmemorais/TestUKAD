using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework.EntityConfigurations
{
    public class TestEntityConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder
                .HasKey(info => info.Id);
            builder
                .Property(info => info.CreateAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
