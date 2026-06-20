using Microsoft.EntityFrameworkCore;
using Purchases.Domain.Entities;
using Purchases.Domain.Enums;
using Purchases.Domain.Repositories;

namespace Purchases.Infrastructure.Persistence.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly PurchasesDbContext _context;

    public PurchaseOrderRepository(PurchasesDbContext context)
    {
        _context = context;
    }

    public Task<PurchaseOrder?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.PurchaseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<PurchaseOrder?> GetByCenAsync(string cen, CancellationToken cancellationToken = default)
    {
        return _context.PurchaseOrders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Cen == cen, cancellationToken);
    }

    public Task<List<PurchaseOrder>> GetPagedAsync(string companyCen, PurchaseStatus? status, int page, int pageSize, bool sortDescending, CancellationToken cancellationToken = default)
    {
        var query = _context.PurchaseOrders
            .Include(x => x.Items)
            .Where(x => x.CompanyCen == companyCen);

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        query = sortDescending 
            ? query.OrderByDescending(x => x.CreatedAt) 
            : query.OrderBy(x => x.CreatedAt);

        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetTotalCountAsync(string companyCen, PurchaseStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _context.PurchaseOrders
            .Where(x => x.CompanyCen == companyCen);

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
    {
        _context.PurchaseOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(PurchaseOrder order, CancellationToken cancellationToken = default)
    {
        _context.PurchaseOrders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
