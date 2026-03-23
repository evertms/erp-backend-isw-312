using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductStock> ProductStocks => Set<ProductStock>();
    public DbSet<InventoryDocument> InventoryDocuments => Set<InventoryDocument>();
    public DbSet<InventoryDocumentLine> InventoryDocumentLines => Set<InventoryDocumentLine>();
    public DbSet<KardexMovement> KardexMovements => Set<KardexMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("inventory");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
