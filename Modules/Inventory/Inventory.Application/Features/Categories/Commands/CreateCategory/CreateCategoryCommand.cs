using MediatR;

namespace Inventory.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    Guid CompanyId,
    string Name,
    string? Description
) : IRequest<Guid>;
