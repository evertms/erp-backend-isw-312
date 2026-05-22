namespace Shared.Contracts.Inventory;

public record StockIncreaseContractRequest(
    string WarehouseCen,
    string Source,
    string ReferenceCen,
    string? Reason,
    List<StockValidationItemContractDto> Items
);
