using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(Guid Id, Guid CompanyId) : IRequest<UnitContractDto?>;
