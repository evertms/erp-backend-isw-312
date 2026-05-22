using Shared.Contracts.Sales;

namespace Sales.API.Endpoints;

public static class TaxConfigurationEndpoints
{
    public static void MapTaxConfigurationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/companies/{companyCen}/tax-configuration").WithTags("TaxConfigurationContract");

        group.MapGet("/", (string companyCen) => Results.Ok(new TaxConfigurationContractResponse(companyCen, 13.0)))
        .Produces<TaxConfigurationContractResponse>(StatusCodes.Status200OK)
        .WithName("GetTaxConfiguration")
        .WithSummary("Obtiene configuracion de impuestos");
    }
}
