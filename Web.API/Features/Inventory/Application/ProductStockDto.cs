namespace Inventory.API.Features.Inventory.Application;

public record ProductStockDto(Guid WarehouseId, decimal CurrentQuantity);
