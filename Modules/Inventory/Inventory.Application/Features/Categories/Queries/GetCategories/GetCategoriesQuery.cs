using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery(Guid CompanyId) : IRequest<List<CategoryDto>>;
