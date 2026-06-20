using Inventory.Application.Features.Units.Commands.CreateUnit;
using Inventory.Application.Features.Units.Commands.UpdateUnit;
using Inventory.Application.Features.Units.Queries.GetUnits;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class UnitEndpoints
{
    public static void MapUnitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/units").WithTags("Inventory Catalog Contract");

        group.MapPost("", async (string companyCen, CreateUnitContractRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new CreateUnitCommand(companyCen, request.Name, request.Abbreviation ?? string.Empty);
                var cen = await mediator.Send(command);
                
                return Results.Created($"/api/inventory/companies/{companyCen}/units/{cen}", new UnitContractDto(cen, request.Name, request.Abbreviation, true));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { Error = ex.Message });
            }
        });

        group.MapPut("/{unitCen}", async (string companyCen, string unitCen, CreateUnitContractRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new UpdateUnitCommand(unitCen, companyCen, request.Name, request.Abbreviation ?? string.Empty);
                var result = await mediator.Send(command);
                
                return result 
                    ? Results.Ok(new UnitContractDto(unitCen, request.Name, request.Abbreviation, true)) 
                    : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { Error = ex.Message });
            }
        });

        group.MapGet("", async (string companyCen, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUnitsQuery(companyCen));
            return Results.Ok(result);
        });
    }
}
