using MediatR;
using Purchases.Application.Services;
using Purchases.Domain.Enums;
using Purchases.Domain.Repositories;
using Shared.Contracts.Purchases;

namespace Purchases.Application.Features.PurchaseOrders.Commands;

public record ConfirmPurchaseOrderCommand(string CompanyCen, string OrderCen) : IRequest<PurchaseOrderConfirmationDto>;

public class ConfirmPurchaseOrderCommandHandler : IRequestHandler<ConfirmPurchaseOrderCommand, PurchaseOrderConfirmationDto>
{
    private readonly IPurchaseOrderRepository _repository;
    private readonly IInventoryIntegrationService _inventoryService;

    public ConfirmPurchaseOrderCommandHandler(IPurchaseOrderRepository repository, IInventoryIntegrationService inventoryService)
    {
        _repository = repository;
        _inventoryService = inventoryService;
    }

    public async Task<PurchaseOrderConfirmationDto> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByCenAsync(request.OrderCen, cancellationToken);
        if (order == null || order.CompanyCen != request.CompanyCen)
        {
            throw new Exception($"Purchase order {request.OrderCen} not found.");
        }

        if (order.Status == Domain.Enums.PurchaseStatus.Confirmed)
        {
            throw new Exception("Order is already confirmed.");
        }

        order.Status = Domain.Enums.PurchaseStatus.Confirmed;
        order.ConfirmedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(order, cancellationToken);

        var itemsToInventory = order.Items.Select(i => (i.ProductCen, i.Quantity)).ToList();
        await _inventoryService.UpdateInventoryFromPurchaseAsync(order.CompanyCen, order.WarehouseCen, itemsToInventory, cancellationToken);

        return new PurchaseOrderConfirmationDto
        {
            OrderCen = order.Cen,
            Status = (Shared.Contracts.Purchases.PurchaseStatus)order.Status,
            ConfirmedAt = order.ConfirmedAt.Value
        };
    }
}
