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
        })
        .Produces<List<Shared.Contracts.Inventory.SellableProductContractDto>>(StatusCodes.Status200OK)
        .WithName("GetSalesCatalogProducts")
        .WithSummary("Lista productos vendibles para ventas")
        .WithDescription("Devuelve productos disponibles para venta en la empresa indicada. Integra con el API de Inventario.");
    }
}
