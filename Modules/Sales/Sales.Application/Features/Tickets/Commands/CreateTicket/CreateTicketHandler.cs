using MediatR;
using Sales.Domain.Entities;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.CreateTicket;

public record CreateTicketCommand(
    string CompanyCen,
    CreateTicketContractRequest Request
) : IRequest<TicketContractResponse>;

public class CreateTicketHandler(
    ITicketRepository ticketRepository,
    ITaxConfigurationRepository taxRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTicketCommand, TicketContractResponse>
{
    public async Task<TicketContractResponse> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var taxConfig = await taxRepository.GetByCompanyCenAsync(command.CompanyCen, cancellationToken);
        var taxRate = taxConfig?.GlobalTaxRate ?? 0m;
        
        var nextNumber = await ticketRepository.GetNextDailyNumberAsync(command.CompanyCen, cancellationToken);

        var ticket = Ticket.Create(
            command.CompanyCen,
            command.Request.WaiterCen ?? string.Empty,
            taxRate,
            nextNumber
        );

        await ticketRepository.AddAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TicketContractResponse(
            ticket.Cen,
            ticket.DailyNumber,
            ticket.Status.ToString(),
            ticket.CreatedAt.ToString("o"),
            ticket.WaiterCen,
            ticket.CompanyCen,
            (double)ticket.TaxAmount
        );
    }
}
