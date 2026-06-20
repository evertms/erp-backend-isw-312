using MediatR;
using Purchases.Domain.Entities;
using Purchases.Domain.Enums;
using Purchases.Domain.Repositories;
using Shared.Contracts.Purchases;

namespace Purchases.Application.Features.PurchaseOrders.Commands;

public record CreatePurchaseOrderCommand(string CompanyCen, CreatePurchaseOrderDto Dto) : IRequest<PurchaseOrderSummaryDto>;

public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, PurchaseOrderSummaryDto>
{
    private readonly IPurchaseOrderRepository _repository;

    public CreatePurchaseOrderCommandHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<PurchaseOrderSummaryDto> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new PurchaseOrder
        {
            Cen = $"PUR-{Guid.CreateVersion7()}",
            CompanyCen = request.CompanyCen,
            SupplierCen = request.Dto.SupplierCen,
            WarehouseCen = request.Dto.WarehouseCen,
            Status = Domain.Enums.PurchaseStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            Items = request.Dto.Items.Select(i => new PurchaseOrderItem
            {
                Cen = $"POI-{Guid.CreateVersion7()}",
                ProductCen = i.ProductCen,
                Quantity = i.Quantity
            }).ToList()
        };

        await _repository.AddAsync(order, cancellationToken);

        return new PurchaseOrderSummaryDto
        {
            OrderCen = order.Cen,
            Status = (Shared.Contracts.Purchases.PurchaseStatus)order.Status
        };
    }
}
