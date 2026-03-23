using MediatR;
using Inventory.Application.Features.Stocks.DTOs;

namespace Inventory.Application.Features.Stocks.Queries.GetProductStock;

public record GetProductStockQuery(Guid ProductId) : IRequest<List<ProductStockDto>>;
