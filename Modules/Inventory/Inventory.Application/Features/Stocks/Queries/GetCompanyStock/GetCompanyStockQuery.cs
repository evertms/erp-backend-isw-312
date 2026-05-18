using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Queries.GetCompanyStock;

public record GetCompanyStockQuery(
    Guid CompanyId,
    Guid? ProductId = null,
    Guid? WarehouseId = null
) : IRequest<List<StockItemContractDto>>;
