namespace Shared.Contracts.Inventory;

public record UpdateProductStatusContractRequest(
    string Status,
    string? Reason
);
