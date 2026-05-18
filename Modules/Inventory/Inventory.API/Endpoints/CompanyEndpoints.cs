using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies").WithTags("Inventory Companies Contract");

        group.MapGet("/", () =>
        {
            return Results.NotFound();
        })
        .Produces<List<CompanyContractDto>>(StatusCodes.Status200OK)
        .WithName("GetInventoryCompanies")
        .WithSummary("Lista empresas del contrato de inventario");

        group.MapGet("/{companyCen}", (string companyCen) =>
        {
            return Results.NotFound();
        })
        .Produces<CompanyLookupContractDto>(StatusCodes.Status200OK)
        .WithName("GetInventoryCompanyByCen")
        .WithSummary("Obtiene una empresa por CEN");
    }
}