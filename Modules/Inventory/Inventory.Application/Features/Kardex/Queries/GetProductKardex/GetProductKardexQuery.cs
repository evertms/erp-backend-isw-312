using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Kardex.Queries.GetProductKardex;

public record GetProductKardexQuery(string ProductCen) : IRequest<List<KardexMovementContractDto>>;
