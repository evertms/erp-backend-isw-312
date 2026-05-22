using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Commands.ConsumeStock;

public record ConsumeStockCommand(
    Guid CompanyId,
    StockConsumeContractRequest Request
) : IRequest<StockConsumeContractResponse>;
