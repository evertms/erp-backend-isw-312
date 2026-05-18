using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery(Guid CompanyId) : IRequest<List<CategoryContractDto>>;
