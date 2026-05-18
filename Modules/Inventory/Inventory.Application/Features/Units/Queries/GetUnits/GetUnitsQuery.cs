using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Units.Queries.GetUnits;

public record GetUnitsQuery(Guid CompanyId) : IRequest<List<UnitContractDto>>;
