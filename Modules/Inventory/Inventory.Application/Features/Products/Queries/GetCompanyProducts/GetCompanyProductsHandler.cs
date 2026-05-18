using MediatR;
using Inventory.Domain.Repositories;
using Inventory.Domain.Enums;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public class GetCompanyProductsHandler(IProductRepository productRepository) : IRequestHandler<GetCompanyProductsQuery, List<ProductContractDto>>
{
    public async Task<List<ProductContractDto>> Handle(GetCompanyProductsQuery request, CancellationToken cancellationToken)
    {
        ProductStatus? statusEnum = null;
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<ProductStatus>(request.Status, true, out var parsedStatus))
        {
            statusEnum = parsedStatus;
        }

        var products = await productRepository.SearchAsync(
            request.CompanyId, 
            request.Search, 
            request.CategoryId, 
            statusEnum, 
            cancellationToken);

        return products.Select(p => new ProductContractDto(
            p.Id.ToString(),
            p.Code ?? string.Empty,
            p.Name,
            null, // Description
            p.CategoryId.ToString(),
            p.Category.Name,
            p.UnitId.ToString(),
            p.Unit.Name,
            (double)p.Price,
            null, // CostPrice
            (double)p.MinStockAlert,
            p.Status.ToString(),
            null // StationCode
        )).ToList();
    }
}
