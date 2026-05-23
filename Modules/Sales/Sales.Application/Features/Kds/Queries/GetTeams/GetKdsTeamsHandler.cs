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

        // Devolvemos el CEN real (STCFG-...) para que el frontend lo use, 
        // pero el nombre amigable (Cocina, Bar) para que el usuario no se pierda.
        return configs
            .Select(c => new KdsTeamContractResponse(
                c.Cen,
                c.Station.ToString(),
                new List<string> { c.CategoryCen }
            ))
            .ToList();
    }
}
