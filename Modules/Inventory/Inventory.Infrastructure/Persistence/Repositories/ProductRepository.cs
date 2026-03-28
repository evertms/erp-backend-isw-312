using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository(InventoryDbContext dbContext) : IProductRepository
{
    public async Task<List<Product>> GetActiveProductsByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.CompanyId == companyId && p.Status == ProductStatus.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetActiveProductsWithStockByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        return await GetActiveProductsByCompanyIdAsync(companyId, cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
    }

    public Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        dbContext.Products.Update(product);
        return Task.CompletedTask;
    }
}
