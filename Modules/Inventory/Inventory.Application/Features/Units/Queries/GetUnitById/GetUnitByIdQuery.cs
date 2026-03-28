using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(Guid Id, Guid CompanyId) : IRequest<UnitDto?>;
