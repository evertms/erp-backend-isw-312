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
        .WithName("GetInventoryCompanies")
        .WithSummary("Lista empresas del contrato de inventario");

        group.MapGet("/{companyCen}", (string companyCen) =>
        {
            return Results.NotFound();
        })
        .WithName("GetInventoryCompanyByCen")
        .WithSummary("Obtiene una empresa por CEN");
    }
}