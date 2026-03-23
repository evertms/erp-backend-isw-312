using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;

namespace Inventory.Infrastructure.Persistence.Repositories;

public class InventoryDocumentRepository(InventoryDbContext dbContext) : IInventoryDocumentRepository
{
    public void Add(InventoryDocument document)
    {
        dbContext.InventoryDocuments.Add(document);
    }
}
