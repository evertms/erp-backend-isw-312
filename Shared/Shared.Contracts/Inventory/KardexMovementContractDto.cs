namespace Shared.Contracts.Inventory;

public record KardexMovementContractDto(
    string MovementCen,
    string? DocumentCen,
    string ProductCen,
    string WarehouseCen,
    string MovementType,
    double Quantity,
    double? UnitCost,
    string? Reason,
    DateTime CreatedAt
);
