using MediatR;
using Inventory.Application.Features.Dashboard.DTOs;
using Inventory.Domain.Repositories;
using System.Linq;

namespace Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;

public class GetDashboardMetricsHandler(IProductRepository productRepository, IProductStockRepository stockRepository) : IRequestHandler<GetDashboardMetricsQuery, DashboardDto>
{
    public async Task<DashboardDto> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        // Nota: Idealmente estas agregaciones deberían hacerse en base de datos.
        // Aquí se hace en memoria como paso de refactorización inicial adaptando los repositorios.
        
        var activeProducts = await productRepository.GetActiveProductsByCompanyIdAsync(request.CompanyId, cancellationToken);
        var totalProductsCount = activeProducts.Count;

        decimal totalStockDecimal = 0;
        int lowStockAlertsCount = 0;

        foreach (var product in activeProducts)
        {
            var stocks = await stockRepository.GetStockByProductIdAsync(product.Id, cancellationToken);
            var productTotalStock = stocks.Sum(s => s.CurrentQuantity);
            
            totalStockDecimal += productTotalStock;

            if (productTotalStock < product.MinStockAlert)
            {
                lowStockAlertsCount++;
            }
        }

        return new DashboardDto(totalProductsCount, (int)totalStockDecimal, lowStockAlertsCount);
    }
}
