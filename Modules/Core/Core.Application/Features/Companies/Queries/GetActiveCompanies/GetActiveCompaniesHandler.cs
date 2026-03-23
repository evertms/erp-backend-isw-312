using MediatR;
using Core.Application.Features.Companies.DTOs;
using Core.Domain.Repositories;

namespace Core.Application.Features.Companies.Queries.GetActiveCompanies;

public class GetActiveCompaniesHandler(ICompanyRepository companyRepository) : IRequestHandler<GetActiveCompaniesQuery, List<CompanyDto>>
{
    public async Task<List<CompanyDto>> Handle(GetActiveCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await companyRepository.GetActiveCompaniesAsync(cancellationToken);
        return companies.Select(c => new CompanyDto(c.Id, c.Name)).ToList();
    }
}
