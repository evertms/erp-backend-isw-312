using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Commands.ValidateStock;

public record ValidateStockCommand(
    string CompanyCen,
    StockValidationContractRequest Request
) : IRequest<StockValidationContractResponse>;
