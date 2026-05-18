using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public class GetCompanyProductsHandler(IProductRepository productRepository) : IRequestHandler<GetCompanyProductsQuery, List<ProductContractDto>>
{
    public async Task<List<ProductContractDto>> Handle(GetCompanyProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetActiveProductsByCompanyIdAsync(request.CompanyId, cancellationToken);
        return products.Select(p => new ProductContractDto(
            p.Id.ToString(),
            p.Code ?? string.Empty,
            p.Name,
            null,
            p.CategoryId.ToString(),
            "", // categoryName - needs join or separate query if not in entity
            p.UnitId.ToString(),
            "", // unitName
            (double)p.Price,
            null,
            (double)p.MinStockAlert,
            p.Status.ToString(),
            null
        )).ToList();
    }
}
