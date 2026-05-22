using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Commands.ConsumeStock;

public record ConsumeStockCommand(
    string CompanyCen,
    StockConsumeContractRequest Request
) : IRequest<StockConsumeContractResponse>;
