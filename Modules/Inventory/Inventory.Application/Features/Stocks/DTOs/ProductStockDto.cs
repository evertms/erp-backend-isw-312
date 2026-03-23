namespace Inventory.Application.Features.Stocks.DTOs;

public record ProductStockDto(Guid WarehouseId, decimal CurrentQuantity);
