namespace Shared.Contracts.Inventory;

public record InventoryAdjustmentContractResponse(
    string AdjustmentCen,
    string Status,
    List<GeneratedMovementContractDto> GeneratedMovements
);
