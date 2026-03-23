using Inventory.Application.Features.Dashboard.Queries.GetDashboardMetrics;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.Infrastructure.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/dashboard").WithTags("Inventory - Dashboard");

        group.MapGet("/{companyId:guid}/metrics", async (Guid companyId, IMediator mediator) =>
        {
            var metrics = await mediator.Send(new GetDashboardMetricsQuery(companyId));
            return Results.Ok(metrics);
        })
        .WithName("GetDashboardMetrics")
        .WithSummary("Retrieves the dashboard metrics for a specific company (total products, in-stock quantity, low stock alerts).");
    }
}
