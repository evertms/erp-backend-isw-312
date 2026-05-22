namespace Shared.Contracts.Inventory;

public record StockConsumeContractResponse(
    bool Success,
    string? DocumentCen,
    string? DocumentType,
    List<string> GeneratedMovementCens,
    List<StockRequirementContractDto> Requirements
);
