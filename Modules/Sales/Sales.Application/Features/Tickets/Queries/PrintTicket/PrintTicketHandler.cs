using MediatR;
using Sales.Domain.Repositories;

namespace Sales.Application.Features.Tickets.Queries.PrintTicket;

public record PrintTicketQuery(
    string CompanyCen,
    string TicketCen
) : IRequest<byte[]>;

public class PrintTicketHandler(ITicketRepository ticketRepository) : IRequestHandler<PrintTicketQuery, byte[]>
{
    public async Task<byte[]> Handle(PrintTicketQuery request, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(request.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != request.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        // Generación simulada de un PDF o TXT de ticket
        var content = $"TICKET: {ticket.DailyNumber}\nTOTAL: {ticket.Total}\nGRACIAS POR SU COMPRA.";
        return System.Text.Encoding.UTF8.GetBytes(content);
    }
}
