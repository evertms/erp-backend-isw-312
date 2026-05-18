namespace Shared.Contracts.Inventory;

public record InventoryDocumentContractDto(
    string DocumentCen,
    string DocumentType,
    string Status,
    string Title,
    DateTime CreatedAt,
    int TotalItems,
    List<string> GeneratedMovementCens
);
