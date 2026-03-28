using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IUnitRepository
{
    Task<List<Unit>> GetAllAsync(Guid companyId, CancellationToken cancellationToken);
    Task<Unit?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsNameUniqueAsync(Guid companyId, string name, CancellationToken cancellationToken);
    Task<bool> IsCodeUniqueAsync(Guid companyId, string code, CancellationToken cancellationToken);
    Task AddAsync(Unit unit, CancellationToken cancellationToken);
    Task UpdateAsync(Unit unit, CancellationToken cancellationToken);
}
