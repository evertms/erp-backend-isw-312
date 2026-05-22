using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IUnitRepository
{
    Task<List<Unit>> GetAllAsync(string companyCen, CancellationToken cancellationToken);
    Task<Unit?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Unit?> GetByCenAsync(string cen, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(string companyCen, string name, CancellationToken cancellationToken);
    Task<bool> IsCodeUniqueAsync(string companyCen, string code, CancellationToken cancellationToken);
    Task AddAsync(Unit unit, CancellationToken cancellationToken);
    Task UpdateAsync(Unit unit, CancellationToken cancellationToken);
}
