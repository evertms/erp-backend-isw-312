namespace Shared.Contracts.Inventory;

public record StockValidationContractRequest(
    string WarehouseCen,
    string Source,
    string? ReferenceCen,
    List<StockValidationItemContractDto> Items
);
