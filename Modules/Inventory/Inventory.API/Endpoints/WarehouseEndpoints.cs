using Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/warehouses").WithTags("Inventory Catalog Contract");

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            var warehouses = await mediator.Send(new GetCompanyWarehousesQuery(companyCen));
            return Results.Ok(warehouses);
        });
    }
}
