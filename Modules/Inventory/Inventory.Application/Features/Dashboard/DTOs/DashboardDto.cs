namespace Inventory.Application.Features.Dashboard.DTOs;

public record DashboardDto(int TotalProducts, int TotalStock, int LowStockAlerts);
