using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Units.Queries.GetUnits;

public class GetUnitsHandler(IUnitRepository unitRepository) : IRequestHandler<GetUnitsQuery, List<UnitContractDto>>
{
    public async Task<List<UnitContractDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await unitRepository.GetAllAsync(request.CompanyId, cancellationToken);

        return units
            .Select(u => new UnitContractDto(u.Id.ToString(), u.Name, u.Code, true))
            .ToList();
    }
}
