namespace Sales.API.Endpoints;

public static class KdsEndpoints
{
    public static void MapKdsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/kds").WithTags("KdsContract");

        group.MapGet("/teams", (string companyCen) => Results.Ok(new List<object>()));
    }
}
