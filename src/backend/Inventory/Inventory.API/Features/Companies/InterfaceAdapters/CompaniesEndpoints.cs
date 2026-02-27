using Inventory.API.Features.Companies.Application;

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
    }
}
