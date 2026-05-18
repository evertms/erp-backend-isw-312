using Inventory.Application.Features.Products.Commands.CreateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProductStatus;
using Inventory.Application.Features.Products.Queries.GetCompanyProducts;
using Inventory.Application.Features.Products.Queries.GetProductById;
using MediatR;

namespace Inventory.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/products").WithTags("Inventory - Products");

        group.MapPost("/", async (string companyCen, CreateProductCommand command, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId) || companyId != command.CompanyId)
                    return Results.BadRequest(new { Error = "CEN de empresa no válido o no coincide con el cuerpo." });

                var id = await mediator.Send(command);
                return Results.Created($"/api/inventory/companies/{companyCen}/products/{id}", new { Id = id });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPost("/lookup", async (string companyCen) =>
        {
            return Results.NotFound();
        })
        .WithName("ProductLookup")
        .WithSummary("Busca productos por CEN dentro de una empresa");

        group.MapPut("/{productCen}", async (string companyCen, string productCen, UpdateProductCommand command, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId) || companyId != command.CompanyId)
                    return Results.BadRequest(new { Error = "CEN de empresa no válido o no coincide con el cuerpo." });

                if (!Guid.TryParse(productCen, out var id) || id != command.Id)
                    return Results.BadRequest(new { Error = "ID en la ruta no coincide con el cuerpo." });

                var result = await mediator.Send(command);
                return result ? Results.NoContent() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPatch("/{productCen}/status", async (string companyCen, string productCen, UpdateProductStatusCommand command, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId) || companyId != command.CompanyId)
                return Results.BadRequest(new { Error = "CEN de empresa no válido o no coincide con el cuerpo." });

            if (!Guid.TryParse(productCen, out var id) || id != command.Id)
                return Results.BadRequest(new { Error = "ID en la ruta no coincide con el cuerpo." });

            var result = await mediator.Send(command);
            return result ? Results.NoContent() : Results.NotFound();
        });

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var products = await mediator.Send(new GetCompanyProductsQuery(companyId));
            return Results.Ok(products);
        })
        .WithName("GetCompanyProducts")
        .WithSummary("Retrieves all active products for a specific company.");

        group.MapGet("/{id:guid}", async (string companyCen, Guid id, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var product = await mediator.Send(new GetProductByIdQuery(id, companyId));
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithSummary("Retrieves a specific product by its ID.");

        app.MapGet("/api/inventory/companies/{companyCen}/sellable-products", async (string companyCen) =>
        {
            return Results.NotFound();
        })
        .WithTags("Inventory - Products")
        .WithName("GetSellableProducts")
        .WithSummary("Lista productos vendibles");
    }
}
