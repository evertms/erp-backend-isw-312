using MediatR;

namespace Inventory.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    string CategoryCen,
    string CompanyCen,
    string Name,
    string? Description
) : IRequest<bool>;
