namespace Sales.API.Endpoints;

public static class TaxConfigurationEndpoints
{
    public static void MapTaxConfigurationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/tax-configuration").WithTags("TaxConfigurationContract");

        group.MapGet("/", (string companyCen) => Results.Ok(new { companyCen, globalTaxPercentage = 13.0 }));
    }
}
