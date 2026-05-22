namespace Shared.Contracts.Inventory;

public record StockConsumeContractRequest(
    string WarehouseCen,
    string Source,
    string ReferenceCen,
    string? Reason,
    List<StockConsumeItemContractDto> Items
);
