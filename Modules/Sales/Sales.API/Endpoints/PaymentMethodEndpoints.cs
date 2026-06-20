using Shared.Contracts.Sales;

namespace Sales.API.Endpoints;

public static class PaymentMethodEndpoints
{
    public static void MapPaymentMethodEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sales/payment-methods").WithTags("PaymentMethodsContract");

        group.MapGet("", () => Results.Ok(new List<PaymentMethodContractResponse>
        {
            new("Efectivo", "Efectivo", true),
            new("Tarjeta", "Tarjeta de Crédito/Débito", true),
            new("Qr", "Pago QR", true)
        }))
        .Produces<List<PaymentMethodContractResponse>>(StatusCodes.Status200OK)
        .WithName("GetPaymentMethods")
        .WithSummary("Lista metodos de pago");
    }
}
