using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IKardexMovementRepository
{
    Task<List<KardexMovement>> GetMovementsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
