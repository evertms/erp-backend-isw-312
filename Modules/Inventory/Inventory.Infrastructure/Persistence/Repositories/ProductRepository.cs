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
        // En un caso real, podría requerir un Include o un query de proyección si el stock está en otro agregado
        // Para respetar la interfaz definida, lo dejamos implementado básico.
        return await GetActiveProductsByCompanyIdAsync(companyId, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
    }
}
