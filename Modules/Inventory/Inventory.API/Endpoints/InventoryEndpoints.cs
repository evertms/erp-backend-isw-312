using Inventory.Application.Features.Documents.Commands.CreateStockAdjustment;
using Inventory.Application.Features.Kardex.Queries.GetCompanyKardexSummaries;
using Inventory.Application.Features.Kardex.Queries.GetProductKardex;
using Inventory.Application.Features.Stocks.Queries.GetProductStock;
using Inventory.Application.Features.Stocks.Queries.GetProductStockInWarehouse;
using Inventory.Application.Features.Stocks.Queries.GetCompanyStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory").WithTags("Inventory");

        group.MapPost("/adjustments", async ([FromBody] CreateStockAdjustmentCommand command, IMediator mediator) =>
        {
            var documentId = await mediator.Send(command);
            return Results.Ok(new { DocumentId = documentId });
        })
        .WithName("CreateStockAdjustment")
        .WithSummary("Registers a new confirmed stock adjustment, immediately generating the kardex movement and modifying stock balance.");

        group.MapGet("/products/{productId:guid}/stock", async (Guid productId, IMediator mediator) =>
        {
            var stock = await mediator.Send(new GetProductStockQuery(productId));
            return Results.Ok(stock);
        })
        .WithName("GetProductStock")
        .WithSummary("Retrieves current stock for a specific product across all warehouses.");

        group.MapGet("/products/{productId:guid}/stock/warehouses/{warehouseId:guid}", async (Guid productId, Guid warehouseId, IMediator mediator) =>
        {
            var stock = await mediator.Send(new GetProductStockInWarehouseQuery(productId, warehouseId));
            return Results.Ok(stock);
        })
        .WithName("GetProductStockInWarehouse")
        .WithSummary("Retrieves current stock for a specific product in a specific warehouse.");

        group.MapGet("/products/{productId:guid}/kardex", async (Guid productId, IMediator mediator) =>
        {
            var kardex = await mediator.Send(new GetProductKardexQuery(productId));
            return Results.Ok(kardex);
        })
        .WithName("GetProductKardex")
        .WithSummary("Retrieves the kardex movement history for a specific product.");

        group.MapGet("/companies/{companyId:guid}/kardex-summaries", async (Guid companyId, IMediator mediator) =>
        {
            var summaries = await mediator.Send(new GetCompanyKardexSummariesQuery(companyId));
            return Results.Ok(summaries);
        })
        .WithName("GetCompanyKardexSummaries")
        .WithSummary("Retrieves a catalog of active products with their total stock, intended as a list to select a product's kardex.");

        // New contract endpoints
        var contractGroup = app.MapGroup("/api/inventory/companies/{companyCen}").WithTags("Inventory Contract");

        contractGroup.MapGet("/stock", async (string companyCen, [FromQuery] string? productCen, [FromQuery] string? warehouseCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            Guid? productId = null;
            if (!string.IsNullOrEmpty(productCen) && Guid.TryParse(productCen, out var pId))
                productId = pId;

            Guid? warehouseId = null;
            if (!string.IsNullOrEmpty(warehouseCen) && Guid.TryParse(warehouseCen, out var wId))
                warehouseId = wId;

            var result = await mediator.Send(new GetCompanyStockQuery(companyId, productId, warehouseId));
            return Results.Ok(result);
        })
        .Produces<List<StockItemContractDto>>(StatusCodes.Status200OK)
        .WithName("GetCompanyStock")
        .WithSummary("Consulta stock por empresa");

        contractGroup.MapGet("/products/{productCen}/kardex", async (string companyCen, string productCen) =>
        {
            return Results.NotFound();
        })
        .Produces<List<KardexMovementContractDto>>(StatusCodes.Status200OK)
        .WithName("GetProductKardexContract")
        .WithSummary("Obtiene kardex de un producto");

        contractGroup.MapPost("/documents", async (string companyCen, InventoryDocumentContractRequest request) =>
        {
            return Results.NotFound();
        })
        .Produces<InventoryDocumentContractDto>(StatusCodes.Status201Created)
        .WithName("CreateInventoryDocument")
        .WithSummary("Crea un documento de inventario");

        contractGroup.MapGet("/documents", async (string companyCen, [FromQuery] string? documentType, [FromQuery] DateTime? from, [FromQuery] DateTime? to) =>
        {
            return Results.NotFound();
        })
        .Produces<List<InventoryDocumentContractDto>>(StatusCodes.Status200OK)
        .WithName("GetInventoryDocuments")
        .WithSummary("Lista documentos de inventario");
    }
}
