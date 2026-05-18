using Inventory.Application.Features.Warehouses.Queries.GetCompanyWarehouses;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/warehouses").WithTags("Inventory - Warehouses");

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var warehouses = await mediator.Send(new GetCompanyWarehousesQuery(companyId));
            return Results.Ok(warehouses);
        })
        .WithName("GetCompanyWarehouses")
        .WithSummary("Retrieves all active warehouses for a specific company.");
    }
}
