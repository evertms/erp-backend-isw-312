using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("suppliers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Cen).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ContactInfo).HasMaxLength(255);
    }
}
