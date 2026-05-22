using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductStockRepository(InventoryDbContext dbContext) : IProductStockRepository
{
    public async Task<List<ProductStock>> GetStockByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductStocks
            .Where(s => s.ProductId == productId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductStock?> GetStockByProductAndWarehouseAsync(int productId, int warehouseId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductStocks
            .FirstOrDefaultAsync(s => s.ProductId == productId && s.WarehouseId == warehouseId, cancellationToken);
    }

    public async Task<List<ProductStock>> GetStockAsync(Guid companyId, int? productId = null, int? warehouseId = null, CancellationToken cancellationToken = default)
    {
        var query = dbContext.ProductStocks
            .Include(ps => ps.Product)
                .ThenInclude(p => p.Unit)
            .Include(ps => ps.Warehouse)
            .Where(ps => ps.Product.CompanyId == companyId);

        if (productId.HasValue)
        {
            query = query.Where(ps => ps.ProductId == productId.Value);
        }

        if (warehouseId.HasValue)
        {
            query = query.Where(ps => ps.WarehouseId == warehouseId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<ProductStock>> GetAllActiveProductsStock(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ProductStocks
            .Include(ps => ps.Product)
            .Where(ps => ps.Product.CompanyId == companyId && ps.Product.Status == ProductStatus.Activo)
            .ToListAsync(cancellationToken);
    }

    public void Add(ProductStock stock)
    {
        dbContext.ProductStocks.Add(stock);
    }
}
