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

    public async Task<List<Product>> SearchAsync(
        Guid companyId, 
        string? searchTerm = null, 
        Guid? categoryId = null, 
        ProductStatus? status = null, 
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyId == companyId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || (p.Code != null && p.Code.ToLower().Contains(term)));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        return await query.ToListAsync(cancellationToken);
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
