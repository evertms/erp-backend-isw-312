using Inventory.Application.Features.Units.Commands.CreateUnit;
using Inventory.Application.Features.Units.Commands.UpdateUnit;
using Inventory.Application.Features.Units.Queries.GetUnits;
using Inventory.Application.Features.Units.Queries.GetUnitById;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class UnitEndpoints
{
    public static void MapUnitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/units").WithTags("Inventory Units");

        group.MapPost("/", async (string companyCen, CreateUnitContractRequest request, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId))
                    return Results.BadRequest(new { Error = "CEN de empresa no válido." });

                var command = new CreateUnitCommand(companyId, request.Name, request.Abbreviation ?? string.Empty);
                var id = await mediator.Send(command);
                
                return Results.Created($"/api/inventory/companies/{companyCen}/units/{id}", new UnitContractDto(id.ToString(), request.Name, request.Abbreviation, true));
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
                if (!Guid.TryParse(companyCen, out var companyId))
                    return Results.BadRequest(new { Error = "CEN de empresa no válido." });

                if (!Guid.TryParse(unitCen, out var id))
                    return Results.BadRequest(new { Error = "CEN de unidad no válido." });

                var command = new UpdateUnitCommand(id, companyId, request.Name, request.Abbreviation ?? string.Empty);
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

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetUnitsQuery(companyId));
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (string companyCen, Guid id, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetUnitByIdQuery(id, companyId));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
