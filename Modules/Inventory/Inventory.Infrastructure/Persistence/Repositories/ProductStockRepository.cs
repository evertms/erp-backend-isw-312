using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductStockRepository(InventoryDbContext dbContext) : IProductStockRepository
{
    public async Task<List<ProductStock>> GetStockByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductStocks
            .Where(s => s.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductStock?> GetStockByProductAndWarehouseAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductStocks
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId, cancellationToken);
    }

    public async Task<List<ProductStock>> GetAllActiveProductsStock(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductStocks
            .Include(ps => ps.Product)
            .Where(ps => ps.Product.CompanyId == companyId && ps.Product.Status == ProductStatus.Activo)
            .ToListAsync(cancellationToken);
    }
}
