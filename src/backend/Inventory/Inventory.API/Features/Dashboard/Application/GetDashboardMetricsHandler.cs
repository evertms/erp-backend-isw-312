using Microsoft.EntityFrameworkCore;
using Inventory.API.Shared.Infrastructure.Persistence.Models;

namespace Inventory.API.Features.Dashboard.Application;

public class GetDashboardMetricsHandler
{
    private readonly AppDbContext _db;

    public GetDashboardMetricsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardDto> HandleAsync(Guid companyId)
    {
        // Total active products for the company
        var totalProducts = await _db.Products
            .CountAsync(p => p.CompanyId == companyId && p.Status == "Activo");

        // Total stock quantity across all warehouses for the company
        var totalStockDecimal = await _db.ProductStocks
            .Where(ps => ps.CompanyId == companyId)
            .SumAsync(ps => (decimal?)ps.CurrentQuantity) ?? 0m;

        var totalStock = (int)totalStockDecimal;

        // Low stock alerts (Products whose sum of stock is below their MinStockAlert)
        var lowStockAlerts = await _db.Products
            .Where(p => p.CompanyId == companyId && p.Status == "Activo")
            .CountAsync(p => 
                (p.ProductStocks.Sum(ps => (decimal?)ps.CurrentQuantity) ?? 0m) < p.MinStockAlert);

        return new DashboardDto(totalProducts, totalStock, lowStockAlerts);
    }
}
