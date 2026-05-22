using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.AssignWaiter;

public record AssignWaiterCommand(
    string CompanyCen,
    string TicketCen,
    AssignTicketWaiterContractRequest Request
) : IRequest<AssignTicketWaiterContractResponse>;

public class AssignWaiterHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AssignWaiterCommand, AssignTicketWaiterContractResponse>
{
    public async Task<AssignTicketWaiterContractResponse> Handle(AssignWaiterCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        ticket.AssignWaiter(command.Request.WaiterCen);

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AssignTicketWaiterContractResponse(
            ticket.Cen,
            ticket.WaiterCen,
            "Waiter Name Placeholder" // In a real scenario, we'd lookup the name from Core
        );
    }
}
