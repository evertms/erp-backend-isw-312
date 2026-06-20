using MediatR;
using Sales.Application.Features.Waiters.Queries.GetWaiters;
using Shared.Contracts.Sales;

namespace Sales.API.Endpoints;

public static class WaitersEndpoints
{
    public static void MapWaitersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/waiters").WithTags("WaitersContract");

        group.MapGet("", async (string companyCen, ISender sender) =>
        {
            var result = await sender.Send(new GetWaitersQuery(companyCen));
            return Results.Ok(result);
        })
        .Produces<List<WaiterContractResponse>>(StatusCodes.Status200OK)
        .WithName("GetWaiters")
        .WithSummary("Lista meseros por empresa");
    }
}
