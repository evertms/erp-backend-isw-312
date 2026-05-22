using Shared.Contracts.Inventory;
using Core.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory/companies").WithTags("Inventory Companies Contract");

        group.MapGet("/", async ([FromServices] ICompanyRepository companyRepository) =>
        {
            var companies = await companyRepository.GetActiveCompaniesAsync();
            return Results.Ok(companies.Select(c => new CompanyContractDto(c.Cen, c.Name, c.IsActive)).ToList());
        })
        .Produces<List<CompanyContractDto>>(StatusCodes.Status200OK)
        .WithName("GetInventoryCompanies")
        .WithSummary("Lista empresas del contrato de inventario");

        group.MapGet("/{companyCen}", async (string companyCen, [FromServices] ICompanyRepository companyRepository) =>
        {
            var companies = await companyRepository.GetActiveCompaniesAsync();
            var company = companies.FirstOrDefault(c => c.Cen == companyCen);

            if (company == null) return Results.NotFound();

            return Results.Ok(new CompanyLookupContractDto(company.Id, company.Cen, company.Name));
        })
        .Produces<CompanyLookupContractDto>(StatusCodes.Status200OK)
        .WithName("GetInventoryCompanyByCen")
        .WithSummary("Obtiene una empresa por CEN");
    }
}
