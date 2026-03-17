using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Inventory.Application;

public class GetProductStockInWarehouseHandler
{
    private readonly AppDbContext _db;

    public GetProductStockInWarehouseHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProductStockDto> HandleAsync(Guid productId, Guid warehouseId)
    {
        var stock = await _db.ProductStocks
            .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.WarehouseId == warehouseId);

        // If the product doesn't have an entry in this warehouse, it means stock is 0.
        return stock != null 
            ? new ProductStockDto(stock.WarehouseId, stock.CurrentQuantity)
            : new ProductStockDto(warehouseId, 0m);
    }
}
