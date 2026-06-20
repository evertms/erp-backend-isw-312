using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Cen).IsUnique();
        
        builder.Property(x => x.Cen).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CompanyCen).IsRequired().HasMaxLength(50);
        builder.Property(x => x.SupplierCen).IsRequired().HasMaxLength(50);
        builder.Property(x => x.WarehouseCen).IsRequired().HasMaxLength(50);
        
        builder.HasMany(x => x.Items)
            .WithOne(x => x.PurchaseOrder)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
