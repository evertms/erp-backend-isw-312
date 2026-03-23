using MediatR;
using Inventory.Application.Features.Stocks.DTOs;

namespace Inventory.Application.Features.Stocks.Queries.GetProductStockInWarehouse;

public record GetProductStockInWarehouseQuery(Guid ProductId, Guid WarehouseId) : IRequest<ProductStockDto>;
