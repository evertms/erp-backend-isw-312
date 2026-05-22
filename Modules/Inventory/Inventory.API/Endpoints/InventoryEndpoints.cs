using Inventory.Application.Features.Documents.Queries.GetInventoryDocuments;
using Inventory.Application.Features.Documents.Commands.CreateInventoryDocument;
using Inventory.Application.Features.Documents.Commands.CreateInventoryAdjustment;
using Inventory.Application.Features.Stocks.Commands.ConsumeStock;
using Inventory.Application.Features.Kardex.Queries.GetProductKardex;
using Inventory.Application.Features.Stocks.Queries.GetCompanyStock;
using Inventory.Application.Features.Stocks.Commands.ValidateStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var contractGroup = app.MapGroup("/api/inventory/companies/{companyCen}").WithTags("Inventory Contract");

        contractGroup.MapGet("/stock", async (string companyCen, [FromQuery] string? productCen, [FromQuery] string? warehouseCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetCompanyStockQuery(companyId, productCen, warehouseCen));
            return Results.Ok(result);
        });

        contractGroup.MapGet("/products/{productCen}/kardex", async (string companyCen, string productCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetProductKardexQuery(productCen));
            return Results.Ok(result);
        });

        contractGroup.MapPost("/documents", async (string companyCen, InventoryDocumentContractRequest request, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new CreateInventoryDocumentCommand(companyId, request));
            return Results.Created($"/api/inventory/companies/{companyCen}/documents/{result.DocumentCen}", result);
        });

        contractGroup.MapPost("/stock/adjustments", async (string companyCen, InventoryAdjustmentContractRequest request, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new CreateInventoryAdjustmentCommand(companyId, request));
            return Results.Created($"/api/inventory/companies/{companyCen}/stock/adjustments/{result.AdjustmentCen}", result);
        });

        contractGroup.MapPost("/stock/validate", async (string companyCen, StockValidationContractRequest request, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new ValidateStockCommand(companyId, request));
            return Results.Ok(result);
        });

        contractGroup.MapGet("/documents", async (string companyCen, [FromQuery] string? documentType, [FromQuery] DateTime? from, [FromQuery] DateTime? to, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetInventoryDocumentsQuery(companyId, documentType, from, to));
            return Results.Ok(result);
        });

        contractGroup.MapPost("/stock/consume", async (string companyCen, StockConsumeContractRequest request, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new ConsumeStockCommand(companyId, request));
            return Results.Ok(result);
        });
    }
}
