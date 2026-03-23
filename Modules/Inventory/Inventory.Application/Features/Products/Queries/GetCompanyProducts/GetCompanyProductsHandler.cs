using MediatR;
using Inventory.Application.Features.Products.DTOs;
using Inventory.Domain.Repositories;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public class GetCompanyProductsHandler(IProductRepository productRepository) : IRequestHandler<GetCompanyProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetCompanyProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetActiveProductsByCompanyIdAsync(request.CompanyId, cancellationToken);
        return products.Select(p => new ProductDto(p.Id, p.Name, p.Code, p.ImageUrl)).ToList();
    }
}
