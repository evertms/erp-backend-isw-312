using Inventory.Domain.Entities;
using Inventory.Domain.Enums;

namespace Inventory.Domain.Repositories;

public interface IInventoryDocumentRepository
{
    void Add(InventoryDocument document);
    Task<List<InventoryDocument>> GetDocumentsAsync(
        Guid companyId, 
        DocumentType? type = null, 
        DateTime? from = null, 
        DateTime? to = null, 
        CancellationToken cancellationToken = default);
}
