using MediatR;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Dashboard.Queries.GetTopProducts;

public record GetTopProductsQuery(string CompanyCen, int TopN) : IRequest<List<TopProductDashboardContractResponse>>;

public class GetTopProductsHandler(ITicketRepository ticketRepository) : IRequestHandler<GetTopProductsQuery, List<TopProductDashboardContractResponse>>
{
    public async Task<List<TopProductDashboardContractResponse>> Handle(GetTopProductsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository.GetDailyTicketsAsync(request.CompanyCen, cancellationToken);
        var paidLines = tickets
            .Where(t => t.Status == TicketStatus.Paid)
            .SelectMany(t => t.Lines)
            .ToList();

        return paidLines
            .GroupBy(l => l.ProductCen)
            .Select(g => new TopProductDashboardContractResponse(
                g.Key,
                g.First().ProductName,
                (int)g.Sum(l => l.Quantity),
                null, // CategoryCen replica not in TicketLine yet, usually we'd add it if needed
                null,
                (double)g.First().UnitPrice
            ))
            .OrderByDescending(p => p.TotalQuantity)
            .Take(request.TopN)
            .ToList();
    }
}
