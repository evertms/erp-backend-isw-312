using Inventory.Application.Features.Categories.Commands.CreateCategory;
using Inventory.Application.Features.Categories.Commands.UpdateCategory;
using Inventory.Application.Features.Categories.Queries.GetCategories;
using Inventory.Application.Features.Categories.Queries.GetCategoryById;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/categories").WithTags("Inventory Categories");

        group.MapPost("/", async (string companyCen, CreateCategoryContractRequest request, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId))
                    return Results.BadRequest(new { Error = "CEN de empresa no válido." });

                var command = new CreateCategoryCommand(companyId, request.Name, request.Description);
                var id = await mediator.Send(command);
                
                return Results.Created($"/api/inventory/companies/{companyCen}/categories/{id}", new CategoryContractDto(id.ToString(), request.Name, request.Description, true));
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapPut("/{categoryCen}", async (string companyCen, string categoryCen, CreateCategoryContractRequest request, IMediator mediator) =>
        {
            try
            {
                if (!Guid.TryParse(companyCen, out var companyId))
                    return Results.BadRequest(new { Error = "CEN de empresa no válido." });

                if (!Guid.TryParse(categoryCen, out var id))
                    return Results.BadRequest(new { Error = "CEN de categoría no válido." });

                var command = new UpdateCategoryCommand(id, companyId, request.Name, request.Description);
                var result = await mediator.Send(command);
                
                return result 
                    ? Results.Ok(new CategoryContractDto(categoryCen, request.Name, request.Description, true)) 
                    : Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = ex.Message });
            }
        });

        group.MapGet("/", async (string companyCen, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetCategoriesQuery(companyId));
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (string companyCen, Guid id, IMediator mediator) =>
        {
            if (!Guid.TryParse(companyCen, out var companyId))
                return Results.BadRequest(new { Error = "CEN de empresa no válido." });

            var result = await mediator.Send(new GetCategoryByIdQuery(id, companyId));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
