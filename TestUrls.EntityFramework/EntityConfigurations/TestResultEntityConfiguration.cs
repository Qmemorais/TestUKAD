using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestUrls.EntityFramework.Entities;

namespace TestUrls.EntityFramework.EntityConfigurations
{
    internal class TestResultEntityConfiguration : IEntityTypeConfiguration<TestResult>
    {
        public void Configure(EntityTypeBuilder<TestResult> builder)
        {
            builder
                .HasKey(entity => entity.Id);
            builder
                .Property(entity => entity.TimeOfResponse)
                .IsRequired();
        }
    }
}
