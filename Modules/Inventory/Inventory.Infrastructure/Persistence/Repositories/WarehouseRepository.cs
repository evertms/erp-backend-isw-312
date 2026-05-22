using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(InventoryDbContext dbContext) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetActiveWarehousesByCompanyIdAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return await dbContext.Warehouses
            .Where(w => w.CompanyCen == companyCen && w.IsActive)
            .ToListAsync(cancellationToken);
    }

    public Task<Warehouse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.Warehouses.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public Task<Warehouse?> GetByCenAsync(string cen, CancellationToken cancellationToken = default)
    {
        return dbContext.Warehouses.FirstOrDefaultAsync(w => w.Cen == cen, cancellationToken);
    }
}
