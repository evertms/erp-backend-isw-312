using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Cen).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Cen).IsUnique();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Phone).HasMaxLength(20);

        builder.HasOne(cu => cu.Company)
            .WithMany(c => c.Customers)
            .HasForeignKey(cu => cu.CompanyId);
    }
}
