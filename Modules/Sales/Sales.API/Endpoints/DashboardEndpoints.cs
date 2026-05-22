using MediatR;
using Sales.Application.Features.Dashboard.Queries.GetDailySales;
using Sales.Application.Features.Dashboard.Queries.GetKdsStatus;
using Sales.Application.Features.Dashboard.Queries.GetTopProducts;

namespace Sales.API.Endpoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/dashboard").WithTags("DashboardContract");

        group.MapGet("/daily-sales", async (string companyCen, ISender sender) =>
        {
            var result = await sender.Send(new GetDailySalesQuery(companyCen));
            return Results.Ok(result);
        });

        group.MapGet("/top-products", async (string companyCen, [Microsoft.AspNetCore.Mvc.FromQuery] int? topN, ISender sender) =>
        {
            var result = await sender.Send(new GetTopProductsQuery(companyCen, topN ?? 10));
            return Results.Ok(result);
        });

        group.MapGet("/kds-status", async (string companyCen, ISender sender) =>
        {
            var result = await sender.Send(new GetKdsStatusQuery(companyCen));
            return Results.Ok(result);
        });
    }
}
