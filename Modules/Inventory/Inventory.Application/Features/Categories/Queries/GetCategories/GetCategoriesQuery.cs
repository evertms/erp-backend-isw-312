using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery(string CompanyCen) : IRequest<List<CategoryContractDto>>;
