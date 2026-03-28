using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync(Guid companyId, CancellationToken cancellationToken);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
}
