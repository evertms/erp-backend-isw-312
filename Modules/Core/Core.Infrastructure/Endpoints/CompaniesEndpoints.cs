using Core.Application.Features.Companies.Queries.GetActiveCompanies;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Core.Infrastructure.Endpoints;

public static class CompaniesEndpoints
{
    public static void MapCompaniesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/companies").WithTags("Core - Companies");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var companies = await mediator.Send(new GetActiveCompaniesQuery());
            return Results.Ok(companies);
        })
        .WithName("GetActiveCompanies")
        .WithSummary("Retrieves all active companies (tenants) available.");
    }
}
