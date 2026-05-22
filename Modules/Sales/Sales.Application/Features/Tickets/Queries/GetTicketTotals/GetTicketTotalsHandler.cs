using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Queries.GetTicketTotals;

public record GetTicketTotalsQuery(
    string CompanyCen,
    string TicketCen
) : IRequest<TicketTotalsContractResponse>;

public class GetTicketTotalsHandler(ITicketRepository ticketRepository) : IRequestHandler<GetTicketTotalsQuery, TicketTotalsContractResponse>
{
    public async Task<TicketTotalsContractResponse> Handle(GetTicketTotalsQuery request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(request.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != request.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        return new TicketTotalsContractResponse(
            ticket.Cen,
            (double)ticket.Subtotal,
            (double)ticket.TaxAmount,
            (double)ticket.Total
        );
    }
}
