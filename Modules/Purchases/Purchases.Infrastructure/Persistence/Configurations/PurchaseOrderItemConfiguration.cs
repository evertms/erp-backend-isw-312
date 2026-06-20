using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Persistence.Configurations;

public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Cen).IsUnique();
        
        builder.Property(x => x.Cen).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ProductCen).IsRequired().HasMaxLength(50);
    }
}
