using MediatR;

namespace Inventory.Application.Features.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    string ProductCen,
    Guid CompanyId,
    string Name,
    string CategoryCen,
    string UnitCen,
    decimal Price,
    string? Code,
    string? SupplierCen,
    string? ImageUrl,
    decimal MinStockAlert
) : IRequest<bool>;
