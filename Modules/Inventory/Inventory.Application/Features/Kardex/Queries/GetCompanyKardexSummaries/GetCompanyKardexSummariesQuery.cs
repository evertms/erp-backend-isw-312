using MediatR;
using Inventory.Application.Features.Kardex.DTOs;

namespace Inventory.Application.Features.Kardex.Queries.GetCompanyKardexSummaries;

public record GetCompanyKardexSummariesQuery(Guid CompanyId) : IRequest<List<CompanyKardexSummaryDto>>;
