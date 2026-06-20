using Purchases.Domain.Entities;

namespace Purchases.Domain.Repositories;

public interface IPurchaseOrderRepository
{
    Task<PurchaseOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PurchaseOrder?> GetByCenAsync(string cen, CancellationToken cancellationToken = default);
    Task<List<PurchaseOrder>> GetPagedAsync(string companyCen, Enums.PurchaseStatus? status, int page, int pageSize, bool sortDescending, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(string companyCen, Enums.PurchaseStatus? status, CancellationToken cancellationToken = default);
    Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default);
    Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken = default);
}
