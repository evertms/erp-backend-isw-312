namespace Inventory.API.Features.Dashboard.Application;

public record DashboardDto(
    int TotalProducts,
    int TotalStock,
    int LowStockAlerts
);
