using MediatR;
using Inventory.Application.Features.Stocks.DTOs;
using Inventory.Domain.Repositories;

namespace Inventory.Application.Features.Stocks.Queries.GetProductStock;

public class GetProductStockHandler(IProductStockRepository stockRepository) : IRequestHandler<GetProductStockQuery, List<ProductStockDto>>
{
    public async Task<List<ProductStockDto>> Handle(GetProductStockQuery request, CancellationToken cancellationToken)
    {
        var stocks = await stockRepository.GetStockByProductIdAsync(request.ProductId, cancellationToken);
        return stocks.Select(s => new ProductStockDto(s.WarehouseId, s.CurrentQuantity)).ToList();
    }
}
