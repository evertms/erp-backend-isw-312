using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class SalesConfigurationConfiguration : IEntityTypeConfiguration<SalesConfiguration>
{
    public void Configure(EntityTypeBuilder<SalesConfiguration> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Cen).IsUnique();

        builder.Property(x => x.CompanyCen).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.CompanyCen).IsUnique();

        builder.Property(x => x.DefaultWarehouseCen).IsRequired().HasMaxLength(50);
    }
}
