namespace Inventory.API.Features.Inventory.Application;

public record CreateStockAdjustmentRequest(
    Guid CompanyId,
    Guid WarehouseId,
    string Notes,
    List<StockAdjustmentLineDto> Lines
);

public record StockAdjustmentLineDto(
    Guid ProductId,
    decimal QuantityDifference // Positive to add stock, negative to reduce stock
);
