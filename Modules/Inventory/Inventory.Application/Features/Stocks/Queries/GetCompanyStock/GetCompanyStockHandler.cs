using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Queries.GetCompanyStock;

public class GetCompanyStockHandler(
    IProductStockRepository stockRepository,
    IProductRepository productRepository,
    IWarehouseRepository warehouseRepository) : IRequestHandler<GetCompanyStockQuery, List<StockItemContractDto>>
{
    public async Task<List<StockItemContractDto>> Handle(GetCompanyStockQuery request, CancellationToken cancellationToken)
    {
        int? productId = null;
        if (!string.IsNullOrEmpty(request.ProductCen))
        {
            var product = await productRepository.GetByCenAsync(request.ProductCen, cancellationToken);
            productId = product?.Id;
            if (productId == null) return new List<StockItemContractDto>();
        }

        int? warehouseId = null;
        if (!string.IsNullOrEmpty(request.WarehouseCen))
        {
            var warehouse = await warehouseRepository.GetByCenAsync(request.WarehouseCen, cancellationToken);
            warehouseId = warehouse?.Id;
            if (warehouseId == null) return new List<StockItemContractDto>();
        }

        var stocks = await stockRepository.GetStockAsync(request.CompanyId, productId, warehouseId, cancellationToken);

        return stocks.Select(s => new StockItemContractDto(
            s.Product.Cen,
            s.Product.Name,
            s.Warehouse.Cen,
            s.Warehouse.Name,
            (double)s.CurrentQuantity,
            0, // ReservedQuantity not implemented yet
            s.Product.Unit.Name,
            (double)s.Product.MinStockAlert,
            s.CurrentQuantity < s.Product.MinStockAlert
        )).ToList();
    }
}
