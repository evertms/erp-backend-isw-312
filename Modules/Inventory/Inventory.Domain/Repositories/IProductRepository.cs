using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetActiveProductsByCompanyIdAsync(string companyCen, CancellationToken cancellationToken = default);
    Task<List<Product>> GetActiveProductsWithStockByCompanyIdAsync(string companyCen, CancellationToken cancellationToken = default);
    Task<List<Product>> SearchAsync(
        string companyCen, 
        string? searchTerm = null, 
        int? categoryId = null, 
        ProductStatus? status = null, 
        CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Product?> GetByCenAsync(string cen, CancellationToken cancellationToken);
    Task<List<Product>> GetByCensAsync(string companyCen, List<string> cens, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
}
