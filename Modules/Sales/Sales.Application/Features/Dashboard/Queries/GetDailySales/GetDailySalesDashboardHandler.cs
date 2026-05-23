using MediatR;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Dashboard.Queries.GetDailySales;

public record GetDailySalesQuery(string CompanyCen) : IRequest<DailySalesDashboardDto>;

public class GetDailySalesDashboardHandler(ITicketRepository ticketRepository) : IRequestHandler<GetDailySalesQuery, DailySalesDashboardDto>
{
    public async Task<DailySalesDashboardDto> Handle(GetDailySalesQuery request, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository.GetDailyTicketsAsync(request.CompanyCen, cancellationToken);
        var paidTickets = tickets.Where(t => t.Status == TicketStatus.Paid).ToList();

        var totalSales = (double)paidTickets.Sum(t => t.Total);
        var count = paidTickets.Count;
        var average = count > 0 ? totalSales / count : 0;

        return new DailySalesDashboardDto(totalSales, count, average);
    }
}
