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
        Guid companyId, 
        DocumentType? type = null, 
        DateTime? from = null, 
        DateTime? to = null, 
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.InventoryDocuments
            .Include(d => d.Lines)
            .Where(d => d.CompanyId == companyId);

        if (type.HasValue)
            query = query.Where(d => d.Type == type.Value);

        if (from.HasValue)
            query = query.Where(d => d.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(d => d.CreatedAt <= to.Value);

        return await query.OrderByDescending(d => d.CreatedAt).ToListAsync(cancellationToken);
    }
}
