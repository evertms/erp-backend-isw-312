using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Units.Queries.GetUnitById;

public class GetUnitByIdHandler(IUnitRepository unitRepository) : IRequestHandler<GetUnitByIdQuery, UnitContractDto?>
{
    public async Task<UnitContractDto?> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.GetByIdAsync(request.Id, cancellationToken);

        if (unit == null || unit.CompanyId != request.CompanyId)
        {
            return null;
        }

        return new UnitContractDto(unit.Id.ToString(), unit.Name, unit.Code, true);
    }
}
