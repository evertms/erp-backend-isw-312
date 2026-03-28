using MediatR;

namespace Inventory.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    Guid CompanyId,
    string Name,
    Guid CategoryId,
    Guid UnitId,
    decimal Price,
    string? Code,
    Guid? SupplierId,
    string? ImageUrl,
    decimal MinStockAlert
) : IRequest<bool>;
