using MediatR;
using Sales.Application.Services;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Inventory;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.PayTicket;

public record PayTicketCommand(
    string CompanyCen,
    string TicketCen,
    PayTicketContractRequest Request
) : IRequest<PayTicketContractResponse>;

public class PayTicketHandler(
    ITicketRepository ticketRepository,
    IInventoryIntegrationService inventoryService,
    IUnitOfWork unitOfWork) : IRequestHandler<PayTicketCommand, PayTicketContractResponse>
{
    public async Task<PayTicketContractResponse> Handle(PayTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        if (ticket.Status == TicketStatus.Paid)
            throw new InvalidOperationException("El ticket ya ha sido pagado.");

        // 1. Validate Stock
        var validationRequest = new StockValidationContractRequest(
            "WH-DEFAULT-OR-DYNAMIC", // In a real scenario, determine the warehouse
            "SalesModule",
            ticket.Cen,
            ticket.Lines.Select(l => new StockValidationItemContractDto(l.ProductCen, (double)l.Quantity)).ToList()
        );

        var validationResponse = await inventoryService.ValidateStockAsync(command.CompanyCen, validationRequest, cancellationToken);
        
        if (!validationResponse.IsValid)
        {
            // The contract says conflict for insufficient stock, but usually Handlers return a result object or throw.
            // Here I'll throw a custom exception that the controller can map to 409.
            throw new StockInsufficiencyException(validationResponse.Requirements
                .Where(r => r.MissingQuantity > 0)
                .Select(r => new StockInsufficiencyResponseDto(null, r.ProductCen, r.ProductName, r.WarehouseCen, (int)r.RequestedQuantity, (int)r.AvailableQuantity, (int)r.MissingQuantity))
                .ToList());
        }

        // 2. Process Local Payment
        if (!Enum.TryParse<PaymentMethod>(command.Request.PaymentMethodCode, true, out var method))
        {
            method = PaymentMethod.Efectivo; // Default or throw
        }

        ticket.Pay(method, ticket.Total);

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Consume Stock
        var consumeRequest = new StockConsumeContractRequest(
            "WH-DEFAULT-OR-DYNAMIC",
            "SalesModule",
            ticket.Cen,
            $"Venta ticket {ticket.Cen}",
            ticket.Lines.Select(l => new StockConsumeItemContractDto(l.ProductCen, (double)l.Quantity)).ToList()
        );

        var consumeResponse = await inventoryService.ConsumeStockAsync(command.CompanyCen, consumeRequest, cancellationToken);

        return new PayTicketContractResponse(
            ticket.Payments.FirstOrDefault()?.Cen ?? "PAY-NEW",
            ticket.Cen,
            ticket.Status.ToString(),
            (double)ticket.Subtotal,
            (double)ticket.TaxAmount,
            (double)ticket.Total,
            consumeResponse.DocumentCen
        );
    }
}

public class StockInsufficiencyException(List<StockInsufficiencyResponseDto> insufficiencies) : Exception("Insufficient stock")
{
    public List<StockInsufficiencyResponseDto> Insufficiencies { get; } = insufficiencies;
}
