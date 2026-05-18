using Inventory.Application.Features.Products.Commands.CreateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProductStatus;
using Inventory.Application.Features.Products.Queries.GetCompanyProducts;
using Inventory.Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/products").WithTags("Inventory - Products");

        group.MapPost("/", async (string companyCen, CreateProductContractRequest request, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId))
                    return Results.BadRequest(new { Error = "CEN de empresa no válido." });

                var command = new CreateProductCommand(
                    companyId, 
                    request.Name, 
                    Guid.Empty, // TODO: Map categoryCen to Guid
                    Guid.Empty, // TODO: Map unitCen to Guid
                    (decimal)request.SalePrice,
                    request.Sku,
                    null,
                    null,
                    (decimal)request.ReorderLevel);

                var id = await mediator.Send(command);
                return Results.Created($"/api/inventory/companies/{companyCen}/products/{id}", new CreateProductContractResponse(id.ToString(), request.Sku, request.Name, "Activo", 0));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPost("/lookup", async (string companyCen, ProductLookupContractRequest request) =>
        {
            return Results.NotFound();
        })
        .WithName("ProductLookup")
        .WithSummary("Busca productos por CEN dentro de una empresa");

        group.MapPut("/{productCen}", async (string companyCen, string productCen, UpdateProductContractRequest request, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId))
                    return Results.BadRequest(new { Error = "CEN de empresa no válido." });

                if (!Guid.TryParse(productCen, out var id))
                    return Results.BadRequest(new { Error = "CEN de producto no válido." });

                var command = new UpdateProductCommand(
                    id,
                    companyId,
                    request.Name,
                    Guid.Empty, // TODO: Map
                    Guid.Empty, // TODO: Map
                    (decimal)request.SalePrice,
                    request.Sku,
                    null,
                    null,
                    (decimal)request.ReorderLevel
                );

                var result = await mediator.Send(command);
                return result ? Results.Ok() : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPatch("/{productCen}/status", async (string companyCen, string productCen, UpdateProductStatusContractRequest request, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            if (!Guid.TryParse(productCen, out var id))
                return Results.BadRequest(new { Error = "CEN de producto no válido." });

            // Note: UpdateProductStatusCommand uses Domain.Enums.ProductStatus, mapping string to enum would be needed here.
            // For now, let's keep it minimal as requested.
            return Results.NotFound();
        });

        group.MapGet("/", async (string companyCen, [FromQuery] string? search, [FromQuery] string? categoryCen, [FromQuery] string? status, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            Guid? categoryId = null;
            if (!string.IsNullOrEmpty(categoryCen) && Guid.TryParse(categoryCen, out var catIdParsed))
                categoryId = catIdParsed;

            var products = await mediator.Send(new GetCompanyProductsQuery(companyId, search, categoryId, status));
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
