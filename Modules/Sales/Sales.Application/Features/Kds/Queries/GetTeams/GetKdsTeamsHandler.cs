using MediatR;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Kds.Queries.GetTeams;

public record GetKdsTeamsQuery(string CompanyCen) : IRequest<List<KdsTeamContractResponse>>;

public class GetKdsTeamsHandler(IStationCategoryConfigRepository configRepository) : IRequestHandler<GetKdsTeamsQuery, List<KdsTeamContractResponse>>
{
    public async Task<List<KdsTeamContractResponse>> Handle(GetKdsTeamsQuery request, CancellationToken cancellationToken)
    {
        var configs = await configRepository.GetByCompanyCenAsync(request.CompanyCen, cancellationToken);

        return configs
            .GroupBy(c => c.Station)
            .Select(g => new KdsTeamContractResponse(
                $"TEAM-{g.Key}", // Virtual team Cen
                g.Key.ToString(),
                g.Select(c => c.CategoryCen).ToList()
            ))
            .ToList();
    }
}
