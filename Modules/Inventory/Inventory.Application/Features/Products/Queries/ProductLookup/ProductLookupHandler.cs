using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.ProductLookup;

public class ProductLookupHandler(IProductRepository productRepository) : IRequestHandler<ProductLookupQuery, List<ProductContractDto>>
{
    public async Task<List<ProductContractDto>> Handle(ProductLookupQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetByCensAsync(request.CompanyCen, request.Request.ProductCens, cancellationToken);

        return products.Select(p => new ProductContractDto(
            p.Cen,
            p.Code ?? string.Empty,
            p.Name,
            null,
            p.Category.Cen,
            p.Category.Name,
            p.Unit.Cen,
            p.Unit.Name,
            (double)p.Price,
            null,
            (double)p.MinStockAlert,
            p.Status.ToString(),
            null
        )).ToList();
    }
}
