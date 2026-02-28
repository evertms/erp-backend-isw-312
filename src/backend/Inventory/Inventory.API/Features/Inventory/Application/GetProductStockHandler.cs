using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Inventory.Application;

public class GetProductStockHandler
{
    private readonly AppDbContext _db;

    public GetProductStockHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductStockDto>> HandleAsync(Guid productId)
    {
        return await _db.ProductStocks
            .Where(ps => ps.ProductId == productId)
            .Select(ps => new ProductStockDto(ps.WarehouseId, ps.CurrentQuantity))
            .ToListAsync();
    }
}
