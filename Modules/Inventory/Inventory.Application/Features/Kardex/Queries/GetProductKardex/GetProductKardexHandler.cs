using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Kardex.Queries.GetProductKardex;

public class GetProductKardexHandler(IKardexMovementRepository kardexRepository) : IRequestHandler<GetProductKardexQuery, List<KardexMovementContractDto>>
{
    public async Task<List<KardexMovementContractDto>> Handle(GetProductKardexQuery request, CancellationToken cancellationToken)
    {
        var movements = await kardexRepository.GetMovementsByProductIdAsync(request.ProductId, cancellationToken);
        return movements.Select(k => new KardexMovementContractDto(
            k.Id.ToString(),
            k.DocumentId?.ToString(),
            k.ProductId.ToString(),
            k.WarehouseId.ToString(),
            k.MovementType.ToString(),
            (double)k.Quantity,
            null, // unitCost
            k.Reason,
            k.MovementDate
        )).ToList();
    }
}
