using MediatR;
using Sales.Application.Services;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Tickets.Commands.AddTicketLine;

public record AddTicketLineCommand(
    string CompanyCen,
    string TicketCen,
    CreateTicketItemContractRequest Request
) : IRequest<TicketItemContractResponse>;

public class AddTicketLineHandler(
    ITicketRepository ticketRepository,
    IInventoryIntegrationService inventoryService,
    IStationCategoryConfigRepository configRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddTicketLineCommand, TicketItemContractResponse>
{
    public async Task<TicketItemContractResponse> Handle(AddTicketLineCommand command, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetByCenAsync(command.TicketCen, cancellationToken);
        if (ticket == null || ticket.CompanyCen != command.CompanyCen)
            throw new ArgumentException("Ticket no encontrado.");

        // Get product details from Inventory
        var product = await inventoryService.GetProductByCenAsync(command.CompanyCen, command.Request.ProductCen, cancellationToken);
        if (product == null)
            throw new ArgumentException("Producto no encontrado en inventario.");

        // 1. Intentar determinar estación desde el código explícito del producto
        Station? station = null;
        if (!string.IsNullOrEmpty(product.StationCode) && Enum.TryParse<Station>(product.StationCode, true, out var parsedStation))
        {
            station = parsedStation;
        }

        // 2. Si es nulo, buscar en la configuración global de la empresa por categoría
        if (station == null && !string.IsNullOrEmpty(product.CategoryCen))
        {
            var configs = await configRepository.GetByCompanyCenAsync(command.CompanyCen, cancellationToken);
            var match = configs.FirstOrDefault(c => c.CategoryCen == product.CategoryCen);
            if (match != null)
            {
                station = match.Station;
            }
        }

        ticket.AddLine(
            product.ProductCen,
            product.Name,
            command.Request.Quantity,
            (decimal)product.SalePrice,
            station,
            command.Request.Note
        );

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var addedLine = ticket.Lines.Last();

        return new TicketItemContractResponse(
            addedLine.Cen,
            addedLine.ProductCen,
            addedLine.ProductName,
            (int)addedLine.Quantity,
            (double)addedLine.UnitPrice,
            addedLine.Notes,
            addedLine.Status.ToString(),
            addedLine.SentAt?.ToString("o"),
            addedLine.ResendCount
        );
    }
}
