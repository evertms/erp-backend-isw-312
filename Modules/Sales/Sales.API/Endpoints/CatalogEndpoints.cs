using Sales.Application.Services;

namespace Sales.API.Endpoints;

public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/catalog").WithTags("CatalogContract");

        group.MapGet("/products", async (string companyCen, string? search, string? categoryCen, string? warehouseCen, bool? onlyAvailable, int? page, int? pageSize, IInventoryIntegrationService inventoryService) =>
        {
            var result = await inventoryService.GetSellableProductsAsync(companyCen, search, categoryCen, warehouseCen, onlyAvailable ?? true, page ?? 1, pageSize ?? 50);
            return Results.Ok(result);
        });
    }
}
