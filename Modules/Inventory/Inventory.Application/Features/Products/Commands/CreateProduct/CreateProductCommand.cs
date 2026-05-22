using MediatR;

namespace Inventory.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    Guid CompanyId,
    string Name,
    string CategoryCen,
    string UnitCen,
    decimal Price,
    string? Code = null,
    string? SupplierCen = null,
    string? ImageUrl = null,
    decimal MinStockAlert = 0
) : IRequest<string>;
