using Inventory.API.Features.Inventory.Application;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Features.Inventory.InterfaceAdapters;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory").WithTags("Inventory");

        group.MapPost("/adjustments", async ([FromBody] CreateStockAdjustmentRequest request, [FromServices] CreateStockAdjustmentHandler handler) =>
        {
            var documentId = await handler.HandleAsync(request);
            return Results.Ok(new { DocumentId = documentId });
        })
        .WithName("CreateStockAdjustment")
        .WithSummary("Registers a new confirmed stock adjustment, immediately generating the kardex movement and modifying stock balance.");

        group.MapGet("/products/{productId:guid}/stock", async (Guid productId, [FromServices] GetProductStockHandler handler) =>
        {
            var stock = await handler.HandleAsync(productId);
            return Results.Ok(stock);
        })
        .WithName("GetProductStock")
        .WithSummary("Retrieves current stock for a specific product across all warehouses.");

        group.MapGet("/products/{productId:guid}/stock/warehouses/{warehouseId:guid}", async (Guid productId, Guid warehouseId, [FromServices] GetProductStockInWarehouseHandler handler) =>
        {
            var stock = await handler.HandleAsync(productId, warehouseId);
            return Results.Ok(stock);
        })
        .WithName("GetProductStockInWarehouse")
        .WithSummary("Retrieves current stock for a specific product in a specific warehouse.");

        group.MapGet("/products/{productId:guid}/kardex", async (Guid productId, [FromServices] GetProductKardexHandler handler) =>
        {
            var kardex = await handler.HandleAsync(productId);
            return Results.Ok(kardex);
        })
        .WithName("GetProductKardex")
        .WithSummary("Retrieves the kardex movement history for a specific product.");

        group.MapGet("/companies/{companyId:guid}/kardex-summaries", async (Guid companyId, [FromServices] GetCompanyKardexSummariesHandler handler) =>
        {
            var summaries = await handler.HandleAsync(companyId);
            return Results.Ok(summaries);
        })
        .WithName("GetCompanyKardexSummaries")
        .WithSummary("Retrieves a catalog of active products with their total stock, intended as a list to select a product's kardex.");
    }
}
