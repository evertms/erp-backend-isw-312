namespace Shared.Contracts.Inventory;

public record StockValidationContractResponse(
    bool IsValid,
    List<StockRequirementContractDto> Requirements
);
