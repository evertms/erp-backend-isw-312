using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(p => p.Cen).IsUnique();

        builder.Property(p => p.Amount).HasPrecision(18, 2);
        builder.Property(p => p.Method).HasConversion<string>();
    }
}

public class TaxConfigurationConfiguration : IEntityTypeConfiguration<TaxConfiguration>
{
    public void Configure(EntityTypeBuilder<TaxConfiguration> builder)
    {
        builder.ToTable("tax_configurations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Cen).IsUnique();

        builder.Property(t => t.CompanyCen).IsRequired().HasMaxLength(50);
        builder.Property(t => t.GlobalTaxRate).HasPrecision(18, 4);
    }
}

public class StationCategoryConfigConfiguration : IEntityTypeConfiguration<StationCategoryConfig>
{
    public void Configure(EntityTypeBuilder<StationCategoryConfig> builder)
    {
        builder.ToTable("station_category_configs");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(s => s.Cen).IsUnique();

        builder.Property(s => s.CompanyCen).IsRequired().HasMaxLength(50);
        builder.Property(s => s.CategoryCen).IsRequired().HasMaxLength(50);
        builder.Property(s => s.Station).HasConversion<string>();
    }
}
