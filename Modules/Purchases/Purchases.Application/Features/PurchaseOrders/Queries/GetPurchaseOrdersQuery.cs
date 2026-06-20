using MediatR;
using Purchases.Domain.Repositories;
using Shared.Contracts.Purchases;

namespace Purchases.Application.Features.PurchaseOrders.Queries;

public record GetPurchaseOrdersQuery(string CompanyCen, Shared.Contracts.Purchases.PurchaseStatus? Status, int Page, int PageSize, bool SortDescending) : IRequest<PagedResultDtoOfPurchaseOrderListDto>;

public class GetPurchaseOrdersQueryHandler : IRequestHandler<GetPurchaseOrdersQuery, PagedResultDtoOfPurchaseOrderListDto>
{
    private readonly IPurchaseOrderRepository _repository;

    public GetPurchaseOrdersQueryHandler(IPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDtoOfPurchaseOrderListDto> Handle(GetPurchaseOrdersQuery request, CancellationToken cancellationToken)
    {
        var domainStatus = request.Status.HasValue ? (Purchases.Domain.Enums.PurchaseStatus)request.Status.Value : (Purchases.Domain.Enums.PurchaseStatus?)null;
        
        var totalCount = await _repository.GetTotalCountAsync(request.CompanyCen, domainStatus, cancellationToken);
        var orders = await _repository.GetPagedAsync(request.CompanyCen, domainStatus, request.Page, request.PageSize, request.SortDescending, cancellationToken);

        return new PagedResultDtoOfPurchaseOrderListDto
        {
            Items = orders.Select(o => new PurchaseOrderListDto
            {
                OrderCen = o.Cen,
                Status = (Shared.Contracts.Purchases.PurchaseStatus)o.Status,
                CreatedAt = o.CreatedAt,
                ConfirmedAt = o.ConfirmedAt,
                SupplierCen = o.SupplierCen,
                ItemCount = o.Items.Count
            }).ToList(),
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
            CurrentPage = request.Page
        };
    }
}
