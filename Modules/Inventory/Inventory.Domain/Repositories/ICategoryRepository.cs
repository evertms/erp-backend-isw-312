using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllCategoriesAsync(CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
}