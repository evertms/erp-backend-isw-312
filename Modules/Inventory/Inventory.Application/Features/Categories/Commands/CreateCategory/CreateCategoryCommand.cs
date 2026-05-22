using MediatR;

namespace Inventory.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string CompanyCen,
    string Name,
    string? Description
) : IRequest<string>;
