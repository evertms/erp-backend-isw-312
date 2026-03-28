using Inventory.Application.DTOs;
using Inventory.Domain.Repositories;
using MediatR;

namespace Inventory.Application.Features.Units.Queries.GetUnitById;

public class GetUnitByIdHandler(IUnitRepository unitRepository) : IRequestHandler<GetUnitByIdQuery, UnitDto?>
{
    public async Task<UnitDto?> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.GetByIdAsync(request.Id, cancellationToken);

        if (unit == null || unit.CompanyId != request.CompanyId)
        {
            return null;
        }

        return new UnitDto(unit.Id, unit.CompanyId, unit.Name, unit.Code);
    }
}
