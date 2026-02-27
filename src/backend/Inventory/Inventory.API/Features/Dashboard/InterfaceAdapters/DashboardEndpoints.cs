using Inventory.API.Features.Dashboard.Application;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Features.Dashboard.InterfaceAdapters;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard").WithTags("Dashboard");

        group.MapGet("/{companyId:guid}/metrics", async (Guid companyId, [FromServices] GetDashboardMetricsHandler handler) =>
        {
            var metrics = await handler.HandleAsync(companyId);
            return Results.Ok(metrics);
        })
        .WithName("GetDashboardMetrics")
        .WithSummary("Retrieves the dashboard metrics for a specific company (total products, in-stock quantity, low stock alerts).");
    }
}
