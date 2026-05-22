using MediatR;
using Sales.Application.Features.Kds.Commands.UpdateItemStatus;
using Sales.Application.Features.Kds.Queries.GetItems;
using Sales.Application.Features.Kds.Queries.GetTeams;
using Shared.Contracts.Sales;

namespace Sales.API.Endpoints;

public static class KdsEndpoints
{
    public static void MapKdsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/kds").WithTags("KdsContract");

        group.MapGet("/teams", async (string companyCen, ISender sender) =>
        {
            var result = await sender.Send(new GetKdsTeamsQuery(companyCen));
            return Results.Ok(result);
        })
        .Produces<List<KdsTeamContractResponse>>(StatusCodes.Status200OK)
        .WithName("GetKdsTeams")
        .WithSummary("Lista equipos KDS");

        group.MapGet("/teams/{teamCen}/items", async (string companyCen, string teamCen, ISender sender) =>
        {
            var result = await sender.Send(new GetKdsItemsQuery(companyCen, teamCen));
            return Results.Ok(result);
        })
        .Produces<List<KdsItemContractResponse>>(StatusCodes.Status200OK)
        .WithName("GetKdsItems")
        .WithSummary("Lista items KDS por equipo");

        group.MapPatch("/items/{ticketItemCen}/status", async (string companyCen, string ticketItemCen, UpdateKdsItemStatusContractRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateKdsItemStatusCommand(companyCen, ticketItemCen, request));
            return result ? Results.Ok() : Results.NotFound();
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("UpdateKdsItemStatus")
        .WithSummary("Actualiza estado de item KDS");
    }
}
