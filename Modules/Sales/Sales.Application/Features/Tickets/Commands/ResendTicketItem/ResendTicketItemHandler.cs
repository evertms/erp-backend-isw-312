using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.ResendTicketItem;

public record ResendTicketItemCommand(
    string CompanyCen,
    string TicketCen,
    string TicketItemCen
) : IRequest<TicketItemContractResponse>;

public class ResendTicketItemHandler(
    ITicketRepository ticketRepository,
    ITicketLineRepository lineRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ResendTicketItemCommand, TicketItemContractResponse>
{
    public async Task<TicketItemContractResponse> Handle(ResendTicketItemCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        var line = await lineRepository.GetByCenAsync(command.TicketItemCen, cancellationToken);
        if (line == null || line.TicketId != ticket.Id)
            throw new ArgumentException("Item no encontrado.");

        line.Resend();

        await lineRepository.UpdateAsync(line, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TicketItemContractResponse(
            line.Cen,
            line.ProductCen,
            line.ProductName,
            (int)line.Quantity,
            (double)line.UnitPrice,
            line.Notes,
            line.Status.ToString(),
            line.SentAt?.ToString("o"),
            line.ResendCount
        );
    }
}
