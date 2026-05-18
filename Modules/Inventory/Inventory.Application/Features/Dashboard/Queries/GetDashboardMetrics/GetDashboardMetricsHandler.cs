using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public class GetDashboardMetricsHandler(IProductStockRepository stockRepository) : IRequestHandler<GetDashboardMetricsQuery, InventoryDashboardContractDto>
{
    public async Task<InventoryDashboardContractDto> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        var activeStocks = await stockRepository.GetAllActiveProductsStock(request.CompanyId, cancellationToken);

        var productGroupedMetrics = activeStocks
            .GroupBy(ps => ps.Product)
            .Select(g => new
            {
                Product = g.Key,
                TotalStock = g.Sum(ps => ps.CurrentQuantity),
                IsLowStock = g.Sum(ps => ps.CurrentQuantity) < g.Key.MinStockAlert    
            })
            .ToList();

        var totalProductsCount = productGroupedMetrics.Count;
        var totalStockDecimal = productGroupedMetrics.Sum(p => p.TotalStock);
        var lowStockAlertsCount = productGroupedMetrics.Count(p => p.IsLowStock);

        return new InventoryDashboardContractDto(
            request.CompanyId.ToString(),
            totalProductsCount,
            (double)totalStockDecimal,
            lowStockAlertsCount,
            0 // outOfStockCount
        );
    }
}
