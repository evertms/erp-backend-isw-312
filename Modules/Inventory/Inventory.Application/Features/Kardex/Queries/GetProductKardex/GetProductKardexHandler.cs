using MediatR;
using Inventory.Domain.Repositories;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Kardex.Queries.GetProductKardex;

public class GetProductKardexHandler(
    IKardexMovementRepository kardexRepository,
    IProductRepository productRepository) : IRequestHandler<GetProductKardexQuery, List<KardexMovementContractDto>>
{
    public async Task<List<KardexMovementContractDto>> Handle(GetProductKardexQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByCenAsync(request.ProductCen, cancellationToken);
        if (product == null) return new List<KardexMovementContractDto>();

        var movements = await kardexRepository.GetMovementsByProductIdAsync(product.Id, cancellationToken);
        
        return movements.Select(k => new KardexMovementContractDto(
            k.Cen,
            k.Document?.Cen,
            k.Product.Cen,
            k.Warehouse.Cen,
            k.MovementType.ToString(),
            (double)k.Quantity,
            null, // unitCost
            k.Reason,
            k.MovementDate
        )).ToList();
    }
}
