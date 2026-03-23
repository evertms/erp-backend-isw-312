using MediatR;
using Inventory.Application.Features.Stocks.DTOs;
using Inventory.Domain.Repositories;

namespace Inventory.Application.Features.Stocks.Queries.GetProductStockInWarehouse;

public class GetProductStockInWarehouseHandler(IProductStockRepository stockRepository) : IRequestHandler<GetProductStockInWarehouseQuery, ProductStockDto>
{
    public async Task<ProductStockDto> Handle(GetProductStockInWarehouseQuery request, CancellationToken cancellationToken)
    {
        var stock = await stockRepository.GetStockByProductAndWarehouseAsync(request.ProductId, request.WarehouseId, cancellationToken);

        return stock != null 
            ? new ProductStockDto(stock.WarehouseId, stock.CurrentQuantity)
            : new ProductStockDto(request.WarehouseId, 0m);
    }
}
