using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Features.Tickets.Commands.CreateTicket;
using Sales.Application.Features.Tickets.Commands.AddTicketLine;
using Sales.Application.Features.Tickets.Commands.AssignWaiter;
using Sales.Application.Features.Tickets.Commands.CancelTicket;
using Sales.Application.Features.Tickets.Commands.PayTicket;
using Sales.Application.Features.Tickets.Queries.GetTicketTotals;
using Shared.Contracts.Sales;

namespace Sales.API.Endpoints;

public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/tickets").WithTags("TicketsContract");

        group.MapPost("/", async (string companyCen, CreateTicketContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateTicketCommand(companyCen, request));
            return Results.Created($"/api/sales/companies/{companyCen}/tickets/{result.TicketCen}", result);
        });

        group.MapGet("/{ticketCen}/totals", async (string companyCen, string ticketCen, ISender sender) =>
        {
            var result = await sender.Send(new GetTicketTotalsQuery(companyCen, ticketCen));
            return Results.Ok(result);
        });

        group.MapPut("/{ticketCen}/waiter", async (string companyCen, string ticketCen, AssignTicketWaiterContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignWaiterCommand(companyCen, ticketCen, request));
            return Results.Ok(result);
        });

        group.MapPost("/{ticketCen}/cancel", async (string companyCen, string ticketCen, [FromBody] CancelTicketContractRequest? request, ISender sender) =>
        {
            var result = await sender.Send(new CancelTicketCommand(companyCen, ticketCen, request));
            return Results.Ok(result);
        });

        group.MapPost("/{ticketCen}/payment", async (string companyCen, string ticketCen, PayTicketContractRequest request, ISender sender) =>
        {
            try
            {
                var result = await sender.Send(new PayTicketCommand(companyCen, ticketCen, request));
                return Results.Ok(result);
            }
            catch (StockInsufficiencyException ex)
            {
                return Results.Conflict(new ProcessRestaurantOrderPaymentResultDto(
                    false, null, null, null, 0, 0, 0, ex.Message, ex.Insufficiencies));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { ex.Message });
            }
        });

        // Items sub-group
        var itemsGroup = group.MapGroup("/{ticketCen}/items");

        itemsGroup.MapPost("/", async (string companyCen, string ticketCen, CreateTicketItemContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AddTicketLineCommand(companyCen, ticketCen, request));
            return Results.Created($"/api/sales/companies/{companyCen}/tickets/{ticketCen}/items/{result.TicketItemCen}", result);
        });
    }
}
