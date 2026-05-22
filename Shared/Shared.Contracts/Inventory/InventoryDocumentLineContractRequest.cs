namespace Shared.Contracts.Inventory;

public record InventoryDocumentLineContractRequest(
    string ProductCen,
    double Quantity,
    double? UnitCost
);
