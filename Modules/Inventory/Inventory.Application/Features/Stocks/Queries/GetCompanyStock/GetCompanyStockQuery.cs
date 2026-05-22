using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Queries.GetCompanyStock;

public record GetCompanyStockQuery(
    string CompanyCen,
    string? ProductCen = null,
    string? WarehouseCen = null
) : IRequest<List<StockItemContractDto>>;
