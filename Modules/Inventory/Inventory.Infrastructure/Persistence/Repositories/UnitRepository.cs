using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class UnitRepository(InventoryDbContext context) : IUnitRepository
{
    public Task<List<Unit>> GetAllAsync(Guid companyId, CancellationToken cancellationToken)
    {
        return context.Units
            .Where(u => u.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }

    public Task<Unit?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return context.Units.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(Guid companyId, string name, CancellationToken cancellationToken)
    {
        return !await context.Units.AnyAsync(u => u.CompanyId == companyId && u.Name == name, cancellationToken);
    }

    public async Task<bool> IsCodeUniqueAsync(Guid companyId, string code, CancellationToken cancellationToken)
    {
        return !await context.Units.AnyAsync(u => u.CompanyId == companyId && u.Code == code, cancellationToken);
    }

    public async Task AddAsync(Unit unit, CancellationToken cancellationToken)
    {
        await context.Units.AddAsync(unit, cancellationToken);
    }

    public Task UpdateAsync(Unit unit, CancellationToken cancellationToken)
    {
        context.Units.Update(unit);
        return Task.CompletedTask;
    }
}
