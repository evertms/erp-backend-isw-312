using MediatR;
using Inventory.Application.Features.Kardex.DTOs;
using Inventory.Domain.Repositories;
using System.Linq;

namespace Inventory.Application.Features.Kardex.Queries.GetCompanyKardexSummaries;

public class GetCompanyKardexSummariesHandler(IProductRepository productRepository, IProductStockRepository stockRepository) : IRequestHandler<GetCompanyKardexSummariesQuery, List<CompanyKardexSummaryDto>>
{
    public async Task<List<CompanyKardexSummaryDto>> Handle(GetCompanyKardexSummariesQuery request, CancellationToken cancellationToken)
    {
        // Nota: Idealmente, esto se debería hacer con una proyección directa desde EF Core para evitar
        // cargar todas las entidades y stocks a memoria, o desde una vista materializada (CQRS Read Model).
        // Adaptación respetando IRepository:
        
        var products = await productRepository.GetActiveProductsByCompanyIdAsync(request.CompanyId, cancellationToken);
        var result = new List<CompanyKardexSummaryDto>();
        
        foreach (var product in products)
        {
            var stocks = await stockRepository.GetStockByProductIdAsync(product.Id, cancellationToken);
            var totalStock = stocks.Sum(s => s.CurrentQuantity);
            
            result.Add(new CompanyKardexSummaryDto(
                product.Id,
                product.Code ?? string.Empty,
                product.Name,
                totalStock
            ));
        }

        return result;
    }
}
