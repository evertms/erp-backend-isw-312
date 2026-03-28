using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id, Guid CompanyId) : IRequest<CategoryDto?>;
