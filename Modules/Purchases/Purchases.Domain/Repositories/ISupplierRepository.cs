using Purchases.Domain.Entities;

namespace Purchases.Domain.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllByCompanyAsync(string companyCen, CancellationToken cancellationToken = default);
    Task<Supplier?> GetByCenAsync(string cen, CancellationToken cancellationToken = default);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default);
}
