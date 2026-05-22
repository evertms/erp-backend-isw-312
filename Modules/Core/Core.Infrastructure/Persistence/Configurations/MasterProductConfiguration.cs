using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Persistence.Configurations;

public class MasterProductConfiguration : IEntityTypeConfiguration<MasterProduct>
{
    public void Configure(EntityTypeBuilder<MasterProduct> builder)
    {
        builder.ToTable("master_products");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Cen).IsUnique();

        builder.Property(x => x.Barcode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Barcode).IsUnique();

        builder.Property(x => x.StandardName).IsRequired().HasMaxLength(255);
    }
}
