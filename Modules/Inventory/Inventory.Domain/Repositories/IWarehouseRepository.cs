using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetActiveWarehousesByCompanyIdAsync(string companyCen, CancellationToken cancellationToken = default);
    Task<Warehouse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Warehouse?> GetByCenAsync(string cen, CancellationToken cancellationToken = default);
}
