namespace Shared.Contracts.Inventory;

public record CreateCategoryContractRequest(
    string Name,
    string? Description
);
