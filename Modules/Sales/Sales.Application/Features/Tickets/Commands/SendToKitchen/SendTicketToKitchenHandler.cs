using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.SendToKitchen;

public record SendTicketToKitchenCommand(string CompanyCen, string TicketCen) : IRequest<List<TicketItemContractResponse>>;

public class SendTicketToKitchenHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<SendTicketToKitchenCommand, List<TicketItemContractResponse>>
{
    public async Task<List<TicketItemContractResponse>> Handle(SendTicketToKitchenCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        // For now, let's assume command number is sequential daily too or just a timestamp
        var commandNumber = (int)(DateTime.UtcNow.Ticks % 10000);
        
        ticket.DispatchPendingLinesToKitchen(commandNumber);

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
