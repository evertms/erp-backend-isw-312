using Inventory.API.Features.Companies.Application;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Features.Companies.InterfaceAdapters;

public static class CompaniesEndpoints
{
    public static void MapCompaniesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").WithTags("Companies");

        group.MapGet("/", async (GetActiveCompaniesHandler handler) =>
        {
            var companies = await handler.HandleAsync();
            return Results.Ok(companies);
        })
        .WithName("GetActiveCompanies")
        .WithSummary("Retrieves all active companies available for operator selection.");

        group.MapGet("/{companyId:guid}/warehouses", async (Guid companyId, [FromServices] GetCompanyWarehousesHandler handler) =>
        {
            var warehouses = await handler.HandleAsync(companyId);
            return Results.Ok(warehouses);
        })
        .WithName("GetCompanyWarehouses")
        .WithSummary("Retrieves all active warehouses for a specific company.");

        group.MapGet("/{companyId:guid}/products", async (Guid companyId, [FromServices] GetCompanyProductsHandler handler) =>
        {
            var products = await handler.HandleAsync(companyId);
            return Results.Ok(products);
        })
        .WithName("GetCompanyProducts")
        .WithSummary("Retrieves all active products for a specific company.");
    }
}
