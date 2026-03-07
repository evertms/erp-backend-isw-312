namespace Inventory.API.Features.Inventory.Application;

public record KardexMovementDto(
    string MovementType,
    DateTime? MovementDate,
    decimal Quantity,
    decimal Balance,
    string? Reason
);
