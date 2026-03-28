using Inventory.Application.DTOs;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdHandler(IProductRepository productRepository) : IRequestHandler<GetProductByIdQuery, ProductDetailsDto?>
{
    public async Task<ProductDetailsDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product == null || product.CompanyId != request.CompanyId)
        {
            return null;
        }

        return new ProductDetailsDto(
            product.Id,
            product.CompanyId,
            product.CategoryId,
            product.UnitId,
            product.SupplierId,
            product.Code,
            product.Name,
            product.Price,
            product.Status,
            product.ImageUrl,
            product.MinStockAlert,
            product.CreatedAt
        );
    }
}
