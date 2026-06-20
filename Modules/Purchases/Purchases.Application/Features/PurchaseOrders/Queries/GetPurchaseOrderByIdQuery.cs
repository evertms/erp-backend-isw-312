using MediatR;
using Purchases.Domain.Repositories;
using Shared.Contracts.Purchases;

namespace Purchases.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrderByIdQuery(string CompanyCen, string OrderCen) : IRequest<PurchaseOrderDetailDto>;

public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDetailDto>
{
    private readonly IPurchaseOrderRepository _repository;

    public GetPurchaseOrderByIdQueryHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<PurchaseOrderDetailDto> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _repository.GetByCenAsync(request.OrderCen, cancellationToken);
        if (order == null || order.CompanyCen != request.CompanyCen)
        {
            throw new Exception($"Purchase order {request.OrderCen} not found.");
        }

        return new PurchaseOrderDetailDto
        {
            OrderCen = order.Cen,
            Status = (Shared.Contracts.Purchases.PurchaseStatus)order.Status,
            CreatedAt = order.CreatedAt,
            ConfirmedAt = order.ConfirmedAt,
            SupplierCen = order.SupplierCen,
            WarehouseCen = order.WarehouseCen,
            Items = order.Items.Select(i => new PurchaseOrderDetailItemDto
            {
                ProductCen = i.ProductCen,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
