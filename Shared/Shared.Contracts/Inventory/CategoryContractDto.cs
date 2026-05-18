namespace Shared.Contracts.Inventory;

public record CategoryContractDto(
    string CategoryCen,
    string Name,
    string? Description,
    bool IsActive
);
