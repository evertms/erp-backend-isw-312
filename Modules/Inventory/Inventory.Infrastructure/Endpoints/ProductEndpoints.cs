using Inventory.Application.Features.Products.Commands.CreateProduct;
using Inventory.Application.Features.Products.Queries.GetCompanyProducts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.Infrastructure.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/products").WithTags("Inventory - Products");

        group.MapPost("/", async (CreateProductCommand command, IMediator mediator) =>
        {
            try
            {
                var id = await mediator.Send(command);
                return Results.Created($"/api/inventory/products/{id}", new { Id = id });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        var readGroup = app.MapGroup("/api/companies/{companyId:guid}/products").WithTags("Inventory - Products");

        readGroup.MapGet("/", async (Guid companyId, IMediator mediator) =>
        {
            var products = await mediator.Send(new GetCompanyProductsQuery(companyId));
            return Results.Ok(products);
        })
        .WithName("GetCompanyProducts")
        .WithSummary("Retrieves all active products for a specific company.");
    }
}
