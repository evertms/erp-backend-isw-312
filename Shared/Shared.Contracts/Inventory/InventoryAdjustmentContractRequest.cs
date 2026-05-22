namespace Shared.Contracts.Inventory;

public record InventoryAdjustmentContractRequest(
    string WarehouseCen,
    string Reason,
    List<InventoryAdjustmentLineContractRequest> Lines
);
