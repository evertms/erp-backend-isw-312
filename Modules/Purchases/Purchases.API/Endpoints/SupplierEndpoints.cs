using MediatR;
using Purchases.Application.Features.Suppliers.Queries;

namespace Purchases.API.Endpoints;

public static class SupplierEndpoints
{
    public static void MapSupplierEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/purchases/companies/{companyCen}/suppliers").WithTags("Supplier");

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSuppliersQuery(companyCen));
            return Results.Ok(result);
        }).WithSummary("Lista proveedores de una empresa");
    }
}
