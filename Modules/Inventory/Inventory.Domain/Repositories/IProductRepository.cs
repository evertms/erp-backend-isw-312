using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetActiveProductsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<List<Product>> GetActiveProductsWithStockByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
    Task<List<Product>> SearchAsync(
        Guid companyId, 
        string? searchTerm = null, 
        int? categoryId = null, 
        ProductStatus? status = null, 
        CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Product?> GetByCenAsync(string cen, CancellationToken cancellationToken);
    Task<List<Product>> GetByCensAsync(Guid companyId, List<string> cens, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task UpdateAsync(Product product, CancellationToken cancellationToken);
}
