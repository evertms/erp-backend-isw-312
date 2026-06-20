using Microsoft.EntityFrameworkCore;
using Purchases.Domain.Entities;
using Purchases.Domain.Repositories;

namespace Purchases.Infrastructure.Persistence.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly PurchasesDbContext _context;

    public SupplierRepository(PurchasesDbContext context)
    {
        _context = context;
    }

    public Task<List<Supplier>> GetAllByCompanyAsync(string companyCen, CancellationToken cancellationToken = default)
    {
        return _context.Suppliers
            .Where(x => x.CompanyCen == companyCen)
            .ToListAsync(cancellationToken);
    }

    public Task<Supplier?> GetByCenAsync(string cen, CancellationToken cancellationToken = default)
    {
        return _context.Suppliers
            .FirstOrDefaultAsync(x => x.Cen == cen, cancellationToken);
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
