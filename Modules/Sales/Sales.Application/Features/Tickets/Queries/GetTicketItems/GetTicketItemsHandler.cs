using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Queries.GetTicketItems;

public record GetTicketItemsQuery(string CompanyCen, string TicketCen) : IRequest<List<TicketItemContractResponse>>;

public class GetTicketItemsHandler(ITicketRepository ticketRepository) : IRequestHandler<GetTicketItemsQuery, List<TicketItemContractResponse>>
{
    public async Task<List<TicketItemContractResponse>> Handle(GetTicketItemsQuery request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(request.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != request.CompanyCen)
            return new List<TicketItemContractResponse>();

        return ticket.Lines.Select(l => new TicketItemContractResponse(
            l.Cen,
            l.ProductCen,
            l.ProductName,
            (int)l.Quantity,
            (double)l.UnitPrice,
            l.Notes,
            l.Status.ToString(),
            l.SentAt?.ToString("o"),
            l.CommandNumber ?? 0
        )).ToList();
    }
}
