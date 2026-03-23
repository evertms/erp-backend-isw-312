using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetActiveWarehousesByCompanyIdAsync(Guid companyId, CancellationToken cancellationToken = default);
}
