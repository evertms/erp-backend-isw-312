using Inventory.Application.DTOs;
using MediatR;

namespace Inventory.Application.Features.Units.Queries.GetUnits;

public record GetUnitsQuery(Guid CompanyId) : IRequest<List<UnitDto>>;
