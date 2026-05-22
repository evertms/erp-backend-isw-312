using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Features.Tickets.Commands.CreateTicket;
using Sales.Application.Features.Tickets.Commands.AddTicketLine;
using Sales.Application.Features.Tickets.Commands.AssignWaiter;
using Sales.Application.Features.Tickets.Commands.CancelTicket;
using Sales.Application.Features.Tickets.Commands.PayTicket;
using Sales.Application.Features.Tickets.Commands.UpdateTicketItem;
using Sales.Application.Features.Tickets.Commands.SendToKitchen;
using Sales.Application.Features.Tickets.Queries.GetTicketTotals;
using Sales.Application.Features.Tickets.Queries.GetDailyTickets;
using Sales.Application.Features.Tickets.Queries.GetTicketItems;
using Shared.Contracts.Sales;

namespace Sales.API.Endpoints;

public static class TicketEndpoints
{
    public static void MapTicketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/tickets").WithTags("TicketsContract");

        group.MapGet("/", async (string companyCen, ISender sender) =>
        {
            var result = await sender.Send(new GetDailyTicketsQuery(companyCen));
            return Results.Ok(result);
        })
        .Produces<List<TicketContractResponse>>(StatusCodes.Status200OK)
        .WithName("GetDailyTickets")
        .WithSummary("Lista tickets del dia");

        group.MapPost("/", async (string companyCen, CreateTicketContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateTicketCommand(companyCen, request));
            return Results.Created($"/api/sales/companies/{companyCen}/tickets/{result.TicketCen}", result);
        })
        .Produces<TicketContractResponse>(StatusCodes.Status201Created)
        .WithName("CreateTicket")
        .WithSummary("Crea un ticket");

        group.MapGet("/{ticketCen}/totals", async (string companyCen, string ticketCen, ISender sender) =>
        {
            var result = await sender.Send(new GetTicketTotalsQuery(companyCen, ticketCen));
            return Results.Ok(result);
        })
        .Produces<TicketTotalsContractResponse>(StatusCodes.Status200OK)
        .WithName("GetTicketTotals")
        .WithSummary("Obtiene totales de un ticket");

        group.MapPut("/{ticketCen}/waiter", async (string companyCen, string ticketCen, AssignTicketWaiterContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AssignWaiterCommand(companyCen, ticketCen, request));
            return Results.Ok(result);
        })
        .Produces<AssignTicketWaiterContractResponse>(StatusCodes.Status200OK)
        .WithName("AssignTicketWaiter")
        .WithSummary("Asigna mesero a un ticket");

        group.MapPost("/{ticketCen}/cancel", async (string companyCen, string ticketCen, [FromBody] CancelTicketContractRequest? request, ISender sender) =>
        {
            var result = await sender.Send(new CancelTicketCommand(companyCen, ticketCen, request));
            return Results.Ok(result);
        })
        .Produces<CancelTicketContractResponse>(StatusCodes.Status200OK)
        .WithName("CancelTicket")
        .WithSummary("Cancela un ticket");

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
        })
        .Produces<PayTicketContractResponse>(StatusCodes.Status200OK)
        .Produces<ProcessRestaurantOrderPaymentResultDto>(StatusCodes.Status409Conflict)
        .WithName("PayTicket")
        .WithSummary("Procesa el pago de un ticket");

        group.MapPost("/{ticketCen}/send", async (string companyCen, string ticketCen, ISender sender) =>
        {
            var result = await sender.Send(new SendTicketToKitchenCommand(companyCen, ticketCen));
            return Results.Ok(result);
        })
        .Produces<List<TicketItemContractResponse>>(StatusCodes.Status200OK)
        .WithName("SendTicketToKitchen")
        .WithSummary("Envia un ticket a cocina");

        // Items sub-group
        var itemsGroup = group.MapGroup("/{ticketCen}/items");

        itemsGroup.MapGet("/", async (string companyCen, string ticketCen, ISender sender) =>
        {
            var result = await sender.Send(new GetTicketItemsQuery(companyCen, ticketCen));
            return Results.Ok(result);
        })
        .Produces<List<TicketItemContractResponse>>(StatusCodes.Status200OK)
        .WithName("GetTicketItems")
        .WithSummary("Lista items de un ticket");

        itemsGroup.MapPost("/", async (string companyCen, string ticketCen, CreateTicketItemContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new AddTicketLineCommand(companyCen, ticketCen, request));
            return Results.Created($"/api/sales/companies/{companyCen}/tickets/{ticketCen}/items/{result.TicketItemCen}", result);
        })
        .Produces<TicketItemContractResponse>(StatusCodes.Status201Created)
        .WithName("AddTicketItem")
        .WithSummary("Agrega un item a un ticket");

        itemsGroup.MapPatch("/{ticketItemCen}", async (string companyCen, string ticketCen, string ticketItemCen, UpdateTicketItemContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTicketItemCommand(companyCen, ticketCen, ticketItemCen, request));
            return Results.Ok(result);
        })
        .Produces<TicketItemContractResponse>(StatusCodes.Status200OK)
        .WithName("UpdateTicketItem")
        .WithSummary("Actualiza un item de ticket");
    }
}
