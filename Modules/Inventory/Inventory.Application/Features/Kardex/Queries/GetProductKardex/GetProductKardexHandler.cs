using MediatR;
using Inventory.Application.Features.Kardex.DTOs;
using Inventory.Domain.Repositories;

namespace Inventory.Application.Features.Kardex.Queries.GetProductKardex;

public class GetProductKardexHandler(IKardexMovementRepository kardexRepository) : IRequestHandler<GetProductKardexQuery, List<KardexMovementDto>>
{
    public async Task<List<KardexMovementDto>> Handle(GetProductKardexQuery request, CancellationToken cancellationToken)
    {
        var movements = await kardexRepository.GetMovementsByProductIdAsync(request.ProductId, cancellationToken);
        return movements.Select(k => new KardexMovementDto(
            k.MovementType.ToString(),
            k.MovementDate,
            k.Quantity,
            k.Balance,
            k.Reason
        )).ToList();
    }
}
