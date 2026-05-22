using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class UnitRepository(InventoryDbContext context) : IUnitRepository
{
    public Task<List<Unit>> GetAllAsync(string companyCen, CancellationToken cancellationToken)
    {
        return context.Units
            .Where(u => u.CompanyCen == companyCen)
            .ToListAsync(cancellationToken);
    }

    public Task<Unit?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return context.Units.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<Unit?> GetByCenAsync(string cen, CancellationToken cancellationToken)
    {
        return context.Units.FirstOrDefaultAsync(u => u.Cen == cen, cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(string companyCen, string name, CancellationToken cancellationToken)
    {
        return !await context.Units.AnyAsync(u => u.CompanyCen == companyCen && u.Name == name, cancellationToken);
    }

    public async Task<bool> IsCodeUniqueAsync(string companyCen, string code, CancellationToken cancellationToken)
    {
        return !await context.Units.AnyAsync(u => u.CompanyCen == companyCen && u.Code == code, cancellationToken);
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
