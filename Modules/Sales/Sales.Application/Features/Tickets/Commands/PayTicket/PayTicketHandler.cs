using MediatR;
using Sales.Application.Services;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Inventory;
using Shared.Contracts.Sales;
using Microsoft.Extensions.Configuration;

namespace Sales.Application.Features.Tickets.Commands.PayTicket;

public record PayTicketCommand(
    string CompanyCen,
    string TicketCen,
    PayTicketContractRequest Request
) : IRequest<PayTicketContractResponse>;

public class PayTicketHandler(
    ITicketRepository ticketRepository,
    IInventoryIntegrationService inventoryService,
    IUnitOfWork unitOfWork,
    IConfiguration configuration) : IRequestHandler<PayTicketCommand, PayTicketContractResponse>
{
    public async Task<PayTicketContractResponse> Handle(PayTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        if (ticket.Status == TicketStatus.Paid)
            throw new InvalidOperationException("El ticket ya ha sido pagado.");

        var warehouseCen = configuration["InventorySettings:DefaultWarehouseCen"] 
            ?? throw new InvalidOperationException("La bodega por defecto no está configurada en las variables de entorno.");

        // 1. Validar Stock (Fase 1 del 2PC)
        var validationRequest = new StockValidationContractRequest(
            warehouseCen,
            "SalesModule",
            ticket.Cen,
            ticket.Lines.Select(l => new StockValidationItemContractDto(l.ProductCen, (double)l.Quantity)).ToList()
        );

        var validationResponse = await inventoryService.ValidateStockAsync(command.CompanyCen, validationRequest, cancellationToken);
        
        if (!validationResponse.IsValid)
        {
            throw new StockInsufficiencyException(validationResponse.Requirements
                .Where(r => r.MissingQuantity > 0)
                .Select(r => new StockInsufficiencyResponseDto(null, r.ProductCen, r.ProductName, r.WarehouseCen, (int)r.RequestedQuantity, (int)r.AvailableQuantity, (int)r.MissingQuantity))
                .ToList());
        }

        // 2. Procesar Pago Localmente con Transacción
        if (!Enum.TryParse<PaymentMethod>(command.Request.PaymentMethodCode, true, out var method))
        {
            throw new ArgumentException($"El método de pago '{command.Request.PaymentMethodCode}' no es válido.");
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try 
        {
            ticket.Pay(method, ticket.Total);

            await ticketRepository.UpdateAsync(ticket, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 3. Consumir Stock (Fase 2 del 2PC - Llamada Externa)
            var consumeRequest = new StockConsumeContractRequest(
                warehouseCen,
                "SalesModule",
                ticket.Cen,
                $"Venta ticket {ticket.Cen}",
                ticket.Lines.Select(l => new StockConsumeItemContractDto(l.ProductCen, (double)l.Quantity)).ToList()
            );

            // Si esto lanza excepción o falla, el catch hará el rollback de la DB local
            var consumeResponse = await inventoryService.ConsumeStockAsync(command.CompanyCen, consumeRequest, cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            var recentPayment = ticket.Payments.LastOrDefault();

            return new PayTicketContractResponse(
                recentPayment?.Cen ?? throw new InvalidOperationException("No se generó el registro de pago en el dominio."),
                ticket.Cen,
                ticket.Status.ToString(),
                (double)ticket.Subtotal,
                (double)ticket.TaxAmount,
                (double)ticket.Total,
                consumeResponse.DocumentCen
            );
        }
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

public class StockInsufficiencyException(List<StockInsufficiencyResponseDto> insufficiencies) : Exception("Insufficient stock")
{
    public List<StockInsufficiencyResponseDto> Insufficiencies { get; } = insufficiencies;
}
