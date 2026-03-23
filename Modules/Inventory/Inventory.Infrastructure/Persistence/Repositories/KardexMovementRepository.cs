using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class KardexMovementRepository(InventoryDbContext dbContext) : IKardexMovementRepository
{
    public async Task<List<KardexMovement>> GetMovementsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await dbContext.KardexMovements
            .Where(k => k.ProductId == productId)
            .OrderByDescending(k => k.MovementDate)
            .ToListAsync(cancellationToken);
    }
}
