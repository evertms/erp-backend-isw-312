using MediatR;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Kds.Commands.UpdateItemStatus;

public record UpdateKdsItemStatusCommand(
    string CompanyCen,
    string TicketItemCen,
    UpdateKdsItemStatusContractRequest Request
) : IRequest<bool>;

public class UpdateKdsItemStatusHandler(
    ITicketLineRepository lineRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateKdsItemStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateKdsItemStatusCommand command, CancellationToken cancellationToken)
    {
        var line = await lineRepository.GetByCenAsync(command.TicketItemCen, cancellationToken);
        if (line == null) return false;

        // Simple status update based on string from contract
        switch (command.Request.Status.ToLower())
        {
            case "preparing":
                // If it's pending, it's already "preparing" when dispatched, but contract allows patching.
                // We'll just ignore for now or implement domain logic if needed.
                break;
            case "delivered":
                line.MarkAsReady(); // Our domain calls it Ready, then Served. 
                line.MarkAsServed();
                break;
            case "ready":
                line.MarkAsReady();
                break;
            case "canceled":
                // line.Cancel(); // Need to implement in domain
                break;
            default:
                throw new ArgumentException($"Estado '{command.Request.Status}' no soportado.");
        }

        await lineRepository.UpdateAsync(line, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
