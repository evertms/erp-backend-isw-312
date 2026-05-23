using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Queries.GetDailyTickets;

public record GetDailyTicketsQuery(string CompanyCen) : IRequest<List<TicketContractResponse>>;

public class GetDailyTicketsHandler(ITicketRepository ticketRepository) : IRequestHandler<GetDailyTicketsQuery, List<TicketContractResponse>>
{
    public async Task<List<TicketContractResponse>> Handle(GetDailyTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await ticketRepository.GetDailyTicketsAsync(request.CompanyCen, cancellationToken);

        return tickets.Select(t => new TicketContractResponse(
            t.Cen,
            t.DailyNumber,
            t.Status.ToString(),
            t.CreatedAt.ToString("o"),
            t.WaiterCen,
            t.CompanyCen,
            (double)t.TaxAmount
        )).ToList();
    }
}
