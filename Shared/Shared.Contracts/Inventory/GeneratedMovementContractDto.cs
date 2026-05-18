namespace Shared.Contracts.Inventory;

public record GeneratedMovementContractDto(
    string MovementCen,
    string ProductCen,
    string WarehouseCen,
    double Quantity,
    string MovementType
);
