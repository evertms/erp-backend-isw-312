using MediatR;

namespace Inventory.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    Guid CompanyId,
    string Name,
    Guid CategoryId,
    Guid UnitId,
    decimal Price,
    string? Code = null,
    Guid? SupplierId = null,
    string? ImageUrl = null,
    decimal MinStockAlert = 0
) : IRequest<Guid>;
