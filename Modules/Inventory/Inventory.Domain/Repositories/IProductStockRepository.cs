using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IProductStockRepository
{
    Task<List<ProductStock>> GetStockByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductStock?> GetStockByProductAndWarehouseAsync(Guid productId, Guid warehouseId, CancellationToken cancellationToken = default);
}
