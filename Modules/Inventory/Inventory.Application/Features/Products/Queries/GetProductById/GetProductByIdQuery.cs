using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id, Guid CompanyId) : IRequest<ProductContractDto?>;
