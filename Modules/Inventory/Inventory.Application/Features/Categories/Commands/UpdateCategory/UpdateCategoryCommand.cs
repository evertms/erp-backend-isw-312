using MediatR;

namespace Inventory.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    string CategoryCen,
    Guid CompanyId,
    string Name,
    string? Description
) : IRequest<bool>;
