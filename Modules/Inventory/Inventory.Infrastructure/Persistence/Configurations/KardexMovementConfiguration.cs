using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Persistence.Configurations;

public class KardexMovementConfiguration : IEntityTypeConfiguration<KardexMovement>
{
    public void Configure(EntityTypeBuilder<KardexMovement> builder)
    {
        builder.ToTable("kardex_movements");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.MovementType).HasConversion<string>();
        builder.Property(x => x.Quantity).HasPrecision(18, 4);
        builder.Property(x => x.Balance).HasPrecision(18, 4);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Document)
            .WithMany()
            .HasForeignKey(x => x.DocumentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
