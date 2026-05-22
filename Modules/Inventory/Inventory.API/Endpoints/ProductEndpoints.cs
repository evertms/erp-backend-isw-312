using Inventory.Application.Features.Products.Commands.CreateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProduct;
using Inventory.Application.Features.Products.Commands.UpdateProductStatus;
using Inventory.Application.Features.Products.Queries.GetCompanyProducts;
using Inventory.Application.Features.Products.Queries.ProductLookup;
using Inventory.Application.Features.Products.Queries.GetSellableProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Inventory;
using Inventory.Domain.Enums;

namespace Inventory.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/products").WithTags("Inventory Products Contract");

        group.MapPost("/", async (string companyCen, CreateProductContractRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new CreateProductCommand(
                    companyCen, 
                    request.Name, 
                    request.CategoryCen, 
                    request.UnitCen, 
                    (decimal)request.SalePrice,
                    request.Sku,
                    null,
                    null,
                    (decimal)request.ReorderLevel);

                var cen = await mediator.Send(command);
                return Results.Created($"/api/inventory/companies/{companyCen}/products/{cen}", new CreateProductContractResponse(cen, request.Sku, request.Name, "Activo", 0));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPut("/{productCen}", async (string companyCen, string productCen, UpdateProductContractRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new UpdateProductCommand(
                    productCen,
                    companyCen,
                    request.Name,
                    request.CategoryCen,
                    request.UnitCen,
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
            if (!Enum.TryParse<ProductStatus>(request.Status, true, out var status))
                return Results.BadRequest(new { Error = "Estado no válido." });

            var command = new UpdateProductStatusCommand(productCen, companyCen, status);
            var result = await mediator.Send(command);
            
            return result ? Results.Ok() : Results.NotFound();
        });

        group.MapGet("/", async (string companyCen, [FromQuery] string? search, [FromQuery] string? categoryCen, [FromQuery] string? status, IMediator mediator) =>
        {
            var products = await mediator.Send(new GetCompanyProductsQuery(companyCen, search, categoryCen, status));
            return Results.Ok(products);
        });

        group.MapPost("/lookup", async (string companyCen, ProductLookupContractRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new ProductLookupQuery(companyCen, request));
            return Results.Ok(result);
        });

        app.MapGet("/api/inventory/companies/{companyCen}/sellable-products", async (string companyCen, [FromQuery] string? search, [FromQuery] string? categoryCen, [FromQuery] string? warehouseCen, [FromQuery] bool? onlyAvailable, [FromQuery] int? page, [FromQuery] int? pageSize, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetSellableProductsQuery(companyCen, search, categoryCen, warehouseCen, onlyAvailable ?? true, page ?? 1, pageSize ?? 50));
            return Results.Ok(result);
        }).WithTags("Inventory Sellable Products Contract");
    }
}
