using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IProductStockRepository
{
    Task<List<ProductStock>> GetStockByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<ProductStock?> GetStockByProductAndWarehouseAsync(int productId, int warehouseId, CancellationToken cancellationToken = default);

    Task<List<ProductStock>> GetStockAsync(Guid companyId, int? productId = null, int? warehouseId = null, CancellationToken cancellationToken = default);
    Task<List<ProductStock>> GetAllActiveProductsStock(Guid companyId, CancellationToken cancellationToken = default);
    void Add(ProductStock stock);
}
