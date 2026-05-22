namespace Shared.Contracts.Sales;

public record StockInsufficiencyResponseDto(
    int? ProductId,
    string? ProductCen,
    string ProductName,
    string? WarehouseCen,
    int RequestedQuantity,
    int AvailableQuantity,
    int MissingQuantity
);
