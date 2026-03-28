using MediatR;

namespace Inventory.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    Guid CompanyId,
    string Name,
    string? Description
) : IRequest<bool>;
