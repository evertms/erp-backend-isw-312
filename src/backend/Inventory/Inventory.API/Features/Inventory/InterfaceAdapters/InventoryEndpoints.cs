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
    }
}
