using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetActiveProductsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<List<Product>> GetActiveProductsWithStockByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken);
}
