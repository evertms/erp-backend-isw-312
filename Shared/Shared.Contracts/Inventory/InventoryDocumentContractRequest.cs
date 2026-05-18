namespace Shared.Contracts.Inventory;

public record InventoryDocumentContractRequest(
    string DocumentType,
    string WarehouseCen,
    string? Reason,
    string? ExternalReference,
    List<InventoryDocumentLineContractRequest> Lines
);
