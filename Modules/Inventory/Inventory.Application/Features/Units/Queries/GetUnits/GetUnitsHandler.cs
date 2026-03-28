using Inventory.Application.DTOs;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Units.Queries.GetUnits;

public class GetUnitsHandler(IUnitRepository unitRepository) : IRequestHandler<GetUnitsQuery, List<UnitDto>>
{
    public async Task<List<UnitDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var units = await unitRepository.GetAllAsync(request.CompanyId, cancellationToken);

        return units
            .Select(u => new UnitDto(u.Id, u.CompanyId, u.Name, u.Code))
            .ToList();
    }
}
