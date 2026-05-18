namespace Shared.Contracts.Inventory;

public record StockItemContractDto(
    string ProductCen,
    string ProductName,
    string WarehouseCen,
    string WarehouseName,
    double AvailableQuantity,
    double ReservedQuantity,
    string UnitName,
    double ReorderLevel,
    bool IsLowStock
);
