using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(InventoryDbContext dbContext) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetActiveWarehousesByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Warehouses
            .Where(w => w.CompanyId == companyId && w.IsActive)
            .ToListAsync(cancellationToken);
    }
}
