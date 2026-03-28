using Inventory.Application.Features.Units.Commands.CreateUnit;
using Inventory.Application.Features.Units.Commands.UpdateUnit;
using Inventory.Application.Features.Units.Queries.GetUnits;
using Inventory.Application.Features.Units.Queries.GetUnitById;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Inventory.Infrastructure.Endpoints;

public static class UnitEndpoints
{
    public static void MapUnitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/units").WithTags("Inventory Units");

        group.MapPost("/", async (CreateUnitCommand command, IMediator mediator) =>
        {
            try
            {
                var id = await mediator.Send(command);
                return Results.Created($"/api/inventory/units/{id}", new { Id = id });
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

        group.MapPut("/{id:guid}", async (Guid id, UpdateUnitCommand command, IMediator mediator) =>
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
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { Error = ex.Message });
            }
        });

        group.MapGet("/{companyId:guid}", async (Guid companyId, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUnitsQuery(companyId));
            return Results.Ok(result);
        });

        group.MapGet("/{companyId:guid}/{id:guid}", async (Guid companyId, Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUnitByIdQuery(id, companyId));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
