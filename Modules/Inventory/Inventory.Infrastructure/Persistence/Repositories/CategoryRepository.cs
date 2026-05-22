using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class CategoryRepository(InventoryDbContext context) : ICategoryRepository
{
    public Task<List<Category>> GetAllCategoriesAsync(string companyCen, CancellationToken cancellationToken)
    {
        return context.Categories
            .Where(c => c.CompanyCen == companyCen)
            .ToListAsync(cancellationToken);
    }

    public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return context.Categories.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<Category?> GetByCenAsync(string cen, CancellationToken cancellationToken)
    {
        return context.Categories.FirstOrDefaultAsync(c => c.Cen == cen, cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        await context.Categories.AddAsync(category, cancellationToken);
    }

    public Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        context.Categories.Update(category);
        return Task.CompletedTask;
    }
}
