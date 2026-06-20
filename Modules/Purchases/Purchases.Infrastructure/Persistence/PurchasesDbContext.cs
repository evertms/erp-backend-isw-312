using Microsoft.EntityFrameworkCore;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Persistence;

public class PurchasesDbContext : DbContext
{
    public PurchasesDbContext(DbContextOptions<PurchasesDbContext> options) : base(options)
    {
    }

    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PurchasesDbContext).Assembly);
    }
}
