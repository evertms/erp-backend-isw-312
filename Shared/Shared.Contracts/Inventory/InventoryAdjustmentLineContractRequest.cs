namespace Shared.Contracts.Inventory;

public record InventoryAdjustmentLineContractRequest(
    string ProductCen,
    double Quantity,
    string AdjustmentType
);
