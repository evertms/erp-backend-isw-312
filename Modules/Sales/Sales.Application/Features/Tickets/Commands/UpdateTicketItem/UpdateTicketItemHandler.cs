using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.UpdateTicketItem;

public record UpdateTicketItemCommand(
    string CompanyCen,
    string TicketCen,
    string TicketItemCen,
    UpdateTicketItemContractRequest Request
) : IRequest<TicketItemContractResponse>;

public class UpdateTicketItemHandler(
    ITicketRepository ticketRepository,
    ITicketLineRepository lineRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateTicketItemCommand, TicketItemContractResponse>
{
    public async Task<TicketItemContractResponse> Handle(UpdateTicketItemCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        var line = await lineRepository.GetByCenAsync(command.TicketItemCen, cancellationToken);
        if (line == null || line.TicketId != ticket.Id)
            throw new ArgumentException("Item no encontrado.");

        line.Update(command.Request.Quantity, command.Request.Note);

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
