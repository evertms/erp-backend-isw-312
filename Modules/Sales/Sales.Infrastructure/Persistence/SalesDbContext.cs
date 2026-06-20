using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Persistence;

public class SalesDbContext(DbContextOptions<SalesDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketLine> TicketLines => Set<TicketLine>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<TaxConfiguration> TaxConfigurations => Set<TaxConfiguration>();
    public DbSet<StationCategoryConfig> StationCategoryConfigs => Set<StationCategoryConfig>();
    public DbSet<SalesConfiguration> SalesConfigurations => Set<SalesConfiguration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("sales");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}
