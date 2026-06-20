namespace Shared.Contracts.Inventory;

public record InventoryDocumentContractDto(
    string DocumentCen,
    string DocumentType,
    string Status,
    string Title,
    DateTime CreatedAt,
    int TotalItems,
    List<string> GeneratedMovementCens,
    string? ProductName = null,
    string? WarehouseName = null,
    double TotalQuantity = 0,
    List<InventoryDocumentLineContractDto>? Lines = null
);

public record InventoryDocumentLineContractDto(
    string ProductCen,
    string ProductName,
    double Quantity
);
