using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("tickets");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(t => t.Cen).IsUnique();

        builder.Property(t => t.CompanyCen).IsRequired().HasMaxLength(50);
        builder.Property(t => t.WaiterCen).HasMaxLength(50);
        builder.Property(t => t.CustomerCen).HasMaxLength(50);

        builder.Property(t => t.Status).HasConversion<string>();
        builder.Property(t => t.Subtotal).HasPrecision(18, 2);
        builder.Property(t => t.TaxAmount).HasPrecision(18, 2);
        builder.Property(t => t.Total).HasPrecision(18, 2);
        builder.Property(t => t.AppliedTaxRate).HasPrecision(18, 4);

        builder.HasMany(t => t.Lines)
            .WithOne(l => l.Ticket)
            .HasForeignKey(l => l.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Payments)
            .WithOne(p => p.Ticket)
            .HasForeignKey(p => p.TicketId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TicketLineConfiguration : IEntityTypeConfiguration<TicketLine>
{
    public void Configure(EntityTypeBuilder<TicketLine> builder)
    {
        builder.ToTable("ticket_lines");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(l => l.Cen).IsUnique();

        builder.Property(l => l.ProductCen).IsRequired().HasMaxLength(50);
        builder.Property(l => l.ProductName).IsRequired().HasMaxLength(255);
        builder.Property(l => l.Quantity).HasPrecision(18, 4);
        builder.Property(l => l.UnitPrice).HasPrecision(18, 2);
        builder.Property(l => l.Status).HasConversion<string>();
        builder.Property(l => l.Station).HasConversion<string>();
    }
}
