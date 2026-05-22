using Inventory.Application.Features.Categories.Commands.CreateCategory;
using Inventory.Application.Features.Categories.Commands.UpdateCategory;
using Inventory.Application.Features.Categories.Queries.GetCategories;
using MediatR;
using Shared.Contracts.Inventory;

namespace Inventory.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies/{companyCen}/categories").WithTags("Inventory Catalog Contract");

        group.MapPost("/", async (string companyCen, CreateCategoryContractRequest request, IMediator mediator) =>
        {
            try
            {
                var command = new CreateCategoryCommand(companyCen, request.Name, request.Description);
                var cen = await mediator.Send(command);
                
                return Results.Created($"/api/inventory/companies/{companyCen}/categories/{cen}", new CategoryContractDto(cen, request.Name, request.Description, true));
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
                var command = new UpdateCategoryCommand(categoryCen, companyCen, request.Name, request.Description);
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
            var result = await mediator.Send(new GetCategoriesQuery(companyCen));
            return Results.Ok(result);
        });
    }
}
