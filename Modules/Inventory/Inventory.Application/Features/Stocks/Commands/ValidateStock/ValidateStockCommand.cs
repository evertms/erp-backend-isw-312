using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.Application.Features.Stocks.Commands.ValidateStock;

public record ValidateStockCommand(
    Guid CompanyId,
    StockValidationContractRequest Request
) : IRequest<StockValidationContractResponse>;
