using Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.Infrastructure.Endpoints;

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies/{companyId:guid}/warehouses").WithTags("Inventory - Warehouses");

        group.MapGet("/", async (Guid companyId, IMediator mediator) =>
        {
            var warehouses = await mediator.Send(new GetCompanyWarehousesQuery(companyId));
            return Results.Ok(warehouses);
        })
        .WithName("GetCompanyWarehouses")
        .WithSummary("Retrieves all active warehouses for a specific company.");
    }
}
