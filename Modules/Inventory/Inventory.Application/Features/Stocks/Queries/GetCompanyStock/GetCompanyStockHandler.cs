using Inventory.Domain.Repositories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Queries.GetCompanyStock;

public class GetCompanyStockHandler(IProductStockRepository stockRepository) : IRequestHandler<GetCompanyStockQuery, List<StockItemContractDto>>
{
    public async Task<List<StockItemContractDto>> Handle(GetCompanyStockQuery request, CancellationToken cancellationToken)
    {
        var stocks = await stockRepository.GetStockAsync(request.CompanyId, request.ProductId, request.WarehouseId, cancellationToken);

        return stocks.Select(s => new StockItemContractDto(
            s.ProductId.ToString(),
            s.Product.Name,
            s.WarehouseId.ToString(),
            s.Warehouse.Name,
            (double)s.CurrentQuantity,
            0, // ReservedQuantity not implemented yet
            s.Product.Unit.Name,
            (double)s.Product.MinStockAlert,
            s.CurrentQuantity < s.Product.MinStockAlert
        )).ToList();
    }
}
