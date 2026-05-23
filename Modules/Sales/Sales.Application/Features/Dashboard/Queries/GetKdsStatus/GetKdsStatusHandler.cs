using MediatR;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Dashboard.Queries.GetKdsStatus;

public record GetKdsStatusQuery(string CompanyCen) : IRequest<KdsStatusDashboardDto>;

public class GetKdsStatusHandler(ITicketRepository ticketRepository) : IRequestHandler<GetKdsStatusQuery, KdsStatusDashboardDto>
{
    public async Task<KdsStatusDashboardDto> Handle(GetKdsStatusQuery request, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository.GetDailyTicketsAsync(request.CompanyCen, cancellationToken);
        var lines = tickets.SelectMany(t => t.Lines).ToList();

        return new KdsStatusDashboardDto(
            lines.Count(l => l.Status == TicketLineStatus.Pending),
            lines.Count(l => l.Status == TicketLineStatus.Preparing),
            lines.Count(l => l.Status == TicketLineStatus.Ready)
        );
    }
}
