using MediatR;
using Inventory.Application.Features.Kardex.DTOs;

namespace Inventory.Application.Features.Kardex.Queries.GetProductKardex;

public record GetProductKardexQuery(Guid ProductId) : IRequest<List<KardexMovementDto>>;
