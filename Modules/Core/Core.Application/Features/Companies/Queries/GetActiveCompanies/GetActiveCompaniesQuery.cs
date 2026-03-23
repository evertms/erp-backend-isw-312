using MediatR;
using Core.Application.Features.Companies.DTOs;

namespace Core.Application.Features.Companies.Queries.GetActiveCompanies;

public record GetActiveCompaniesQuery : IRequest<List<CompanyDto>>;
