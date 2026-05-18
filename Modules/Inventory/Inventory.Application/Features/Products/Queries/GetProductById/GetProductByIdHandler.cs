using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, ProductContractDto?>
{
    public async Task<ProductContractDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null || product.CompanyId != request.CompanyId)
        {
            return null;
        }

        return new ProductContractDto(
            product.Id.ToString(),
            product.Code ?? string.Empty,
            product.Name,
            null,
            product.CategoryId.ToString(),
            "", // categoryName
            product.UnitId.ToString(),
            "", // unitName
            (double)product.Price,
            null,
            (double)product.MinStockAlert,
            product.Status.ToString(),
            null
        );
    }
}
