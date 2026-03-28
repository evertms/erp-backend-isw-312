using Inventory.Application.Features.Products.Commands.CreateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProductStatus;
using Inventory.Application.Features.Products.Queries.GetCompanyProducts;
using Inventory.Application.Features.Products.Queries.GetProductById;
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

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductCommand command, IMediator mediator) =>
        {
            try
            {
                if (id != command.Id)
                    return Results.BadRequest(new { Error = "ID en la ruta no coincide con el cuerpo." });

                var result = await mediator.Send(command);
                return result ? Results.NoContent() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPatch("/{id:guid}/status", async (Guid id, UpdateProductStatusCommand command, IMediator mediator) =>
        {
            if (id != command.Id)
                return Results.BadRequest(new { Error = "ID en la ruta no coincide con el cuerpo." });

            var result = await mediator.Send(command);
            return result ? Results.NoContent() : Results.NotFound();
        });

        var readGroup = app.MapGroup("/api/companies/{companyId:guid}/products").WithTags("Inventory - Products");

        readGroup.MapGet("/", async (Guid companyId, IMediator mediator) =>
        {
            var products = await mediator.Send(new GetCompanyProductsQuery(companyId));
            return Results.Ok(products);
        })
        .WithName("GetCompanyProducts")
        .WithSummary("Retrieves all active products for a specific company.");

        readGroup.MapGet("/{id:guid}", async (Guid companyId, Guid id, IMediator mediator) =>
        {
            var product = await mediator.Send(new GetProductByIdQuery(id, companyId));
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithSummary("Retrieves a specific product by its ID.");
    }
}
