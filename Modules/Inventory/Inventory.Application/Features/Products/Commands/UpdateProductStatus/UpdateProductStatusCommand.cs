using Inventory.Domain.Enums;
using MediatR;

namespace Inventory.Application.Features.Products.Commands.UpdateProductStatus;

public record UpdateProductStatusCommand(
    string ProductCen,
    string CompanyCen,
    ProductStatus Status
) : IRequest<bool>;
