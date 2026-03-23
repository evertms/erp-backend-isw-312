using MediatR;

namespace Inventory.Application.Features.Documents.Commands.CreateStockAdjustment;

public record CreateStockAdjustmentCommand(
    Guid CompanyId,
    Guid WarehouseId,
    string Notes,
    List<StockAdjustmentLineDto> Lines
) : IRequest<Guid>;

public record StockAdjustmentLineDto(
    Guid ProductId,
    decimal QuantityDifference
);
