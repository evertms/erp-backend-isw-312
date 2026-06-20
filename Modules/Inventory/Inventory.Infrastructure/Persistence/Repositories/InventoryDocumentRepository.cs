using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryDocumentRepository(InventoryDbContext dbContext) : IInventoryDocumentRepository
{
    public void Add(InventoryDocument document)
    {
        dbContext.InventoryDocuments.Add(document);
    }

    public async Task<List<InventoryDocument>> GetDocumentsAsync(
        string companyCen, 
        DocumentType? type = null, 
        DateTime? from = null, 
        DateTime? to = null, 
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.InventoryDocuments
            .Include(d => d.Warehouse)
            .Include(d => d.Lines)
                .ThenInclude(l => l.Product)
            .Where(d => d.CompanyCen == companyCen);

        if (type.HasValue)
            query = query.Where(d => d.Type == type.Value);

        if (from.HasValue)
            query = query.Where(d => d.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(d => d.CreatedAt <= to.Value);

        return await query.OrderByDescending(d => d.CreatedAt).ToListAsync(cancellationToken);
    }
}
