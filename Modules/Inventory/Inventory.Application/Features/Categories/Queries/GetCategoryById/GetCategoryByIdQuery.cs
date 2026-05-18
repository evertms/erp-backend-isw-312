using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id, Guid CompanyId) : IRequest<CategoryContractDto?>;
