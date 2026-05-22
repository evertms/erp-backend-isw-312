using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class ProductRepository(InventoryDbContext dbContext) : IProductRepository
{
    public async Task<List<Product>> GetActiveProductsByCompanyIdAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
            .Where(p => p.CompanyCen == companyCen && p.Status == ProductStatus.Activo)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetActiveProductsWithStockByCompanyIdAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return await GetActiveProductsByCompanyIdAsync(companyCen, cancellationToken);
    }

    public async Task<List<Product>> SearchAsync(
        string companyCen, 
        string? searchTerm = null, 
        int? categoryId = null, 
        ProductStatus? status = null, 
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyCen == companyCen);

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

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<Product?> GetByCenAsync(string cen, CancellationToken cancellationToken)
    {
        return dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .FirstOrDefaultAsync(p => p.Cen == cen, cancellationToken);
    }

    public Task<List<Product>> GetByCensAsync(string companyCen, List<string> cens, CancellationToken cancellationToken)
    {
        return dbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyCen == companyCen && cens.Contains(p.Cen))
            .ToListAsync(cancellationToken);
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
