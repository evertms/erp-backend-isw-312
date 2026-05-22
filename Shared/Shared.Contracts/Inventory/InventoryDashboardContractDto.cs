namespace Shared.Contracts.Inventory;

public record InventoryDashboardContractDto(
    string CompanyCen,
    int TotalProducts,
    double TotalStockQuantity,
    int LowStockCount,
    int OutOfStockCount
);
