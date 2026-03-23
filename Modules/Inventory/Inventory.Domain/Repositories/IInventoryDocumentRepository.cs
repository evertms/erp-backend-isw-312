using Inventory.Domain.Entities;

namespace Inventory.Domain.Repositories;

public interface IInventoryDocumentRepository
{
    void Add(InventoryDocument document);
}
