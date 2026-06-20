using MediatR;
using Microsoft.AspNetCore.Mvc;
using Purchases.Application.Features.PurchaseOrders.Commands;
using Purchases.Application.Features.PurchaseOrders.Queries;
using Shared.Contracts.Purchases;

namespace Purchases.API.Endpoints;

public static class PurchaseOrderEndpoints
{
    public static void MapPurchaseOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/purchases/companies/{companyCen}/orders").WithTags("PurchaseOrder");

        group.MapGet("/", async (string companyCen, [FromServices] IMediator mediator, [FromQuery] PurchaseStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool sortDescending = true) =>
        {
            var result = await mediator.Send(new GetPurchaseOrdersQuery(companyCen, status, page, pageSize, sortDescending));
            return Results.Ok(result);
        }).WithSummary("Lista ordenes de compra");

        group.MapPost("/", async (string companyCen, [FromBody] CreatePurchaseOrderDto request, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new CreatePurchaseOrderCommand(companyCen, request));
            return Results.Created($"/api/purchases/companies/{companyCen}/orders/{result.OrderCen}", result);
        }).WithSummary("Crea una orden de compra");

        group.MapGet("/{orderCen}", async (string companyCen, string orderCen, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new GetPurchaseOrderByIdQuery(companyCen, orderCen));
            return Results.Ok(result);
        }).WithSummary("Obtiene el detalle de una orden de compra");

        group.MapPost("/{orderCen}/confirm", async (string companyCen, string orderCen, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(new ConfirmPurchaseOrderCommand(companyCen, orderCen));
            return Results.Ok(result);
        }).WithSummary("Confirma una orden de compra");
    }
}
