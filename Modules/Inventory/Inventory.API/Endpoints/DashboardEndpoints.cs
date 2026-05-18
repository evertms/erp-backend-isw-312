using Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/dashboard").WithTags("Inventory - Dashboard");

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var metrics = await mediator.Send(new GetDashboardMetricsQuery(companyId));
            return Results.Ok(metrics);
        })
        .WithName("GetDashboardMetrics")
        .WithSummary("Retrieves the dashboard metrics for a specific company (total products, in-stock quantity, low stock alerts).");
    }
}
