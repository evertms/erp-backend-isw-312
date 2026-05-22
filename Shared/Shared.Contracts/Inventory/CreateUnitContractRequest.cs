namespace Shared.Contracts.Inventory;

public record CreateUnitContractRequest(
    string Name,
    string? Abbreviation
);
