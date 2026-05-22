using MediatR;
using Inventory.Domain.Repositories;
using Inventory.Domain.Enums;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetCompanyProducts;

public class GetCompanyProductsHandler(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository) : IRequestHandler<GetCompanyProductsQuery, List<ProductContractDto>>
{
    public async Task<List<ProductContractDto>> Handle(GetCompanyProductsQuery request, CancellationToken cancellationToken)
    {
        ProductStatus? statusEnum = null;
        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<ProductStatus>(request.Status, true, out var parsedStatus))
        {
            statusEnum = parsedStatus;
        }

        int? categoryId = null;
        if (!string.IsNullOrEmpty(request.CategoryCen))
        {
            var category = await categoryRepository.GetByCenAsync(request.CategoryCen, cancellationToken);
            if (category != null)
            {
                categoryId = category.Id;
            }
        }

        var products = await productRepository.SearchAsync(
            request.CompanyCen, 
            request.Search, 
            categoryId, 
            statusEnum, 
            cancellationToken);

        return products.Select(p => new ProductContractDto(
            p.Cen,
            p.Code ?? string.Empty,
            p.Name,
            null, // Description
            p.Category.Cen,
            p.Category.Name,
            p.Unit.Cen,
            p.Unit.Name,
            (double)p.Price,
            null, // CostPrice
            (double)p.MinStockAlert,
            p.Status.ToString(),
            null // StationCode
        )).ToList();
    }
}
