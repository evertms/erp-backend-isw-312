using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IKardexMovementRepository
{
    Task<List<KardexMovement>> GetMovementsByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    void Add(KardexMovement movement);
}
