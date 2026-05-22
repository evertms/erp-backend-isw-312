namespace Shared.Contracts.Inventory;

public record UnitContractDto(
    string UnitCen,
    string Name,
    string? Abbreviation,
    bool IsActive
);
