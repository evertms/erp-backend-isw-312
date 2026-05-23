using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.CancelTicket;

public record CancelTicketCommand(
    string CompanyCen,
    string TicketCen,
    CancelTicketContractRequest? Request
) : IRequest<CancelTicketContractResponse>;

public class CancelTicketHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CancelTicketCommand, CancelTicketContractResponse>
{
    public async Task<CancelTicketContractResponse> Handle(CancelTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        ticket.Cancel();

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CancelTicketContractResponse(
            ticket.Cen,
            ticket.Status.ToString()
        );
    }
}
