using MediatR;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Kds.Queries.GetItems;

public record GetKdsItemsQuery(string CompanyCen, string TeamCen) : IRequest<List<KdsItemContractResponse>>;

public class GetKdsItemsHandler(
    ITicketRepository ticketRepository,
    IStationCategoryConfigRepository configRepository) : IRequestHandler<GetKdsItemsQuery, List<KdsItemContractResponse>>
{
    public async Task<List<KdsItemContractResponse>> Handle(GetKdsItemsQuery request, CancellationToken cancellationToken)
    {
        Station station;

        // Soporte dual: CEN real o string virtual (TEAM-...)
        if (request.TeamCen.StartsWith("STCFG-"))
        {
            var config = await configRepository.GetByCenAsync(request.TeamCen, cancellationToken);
            if (config == null || config.CompanyCen != request.CompanyCen)
                throw new ArgumentException($"El equipo KDS '{request.TeamCen}' no es válido.");
            station = config.Station;
        }
        else
        {
            var stationName = request.TeamCen.Replace("TEAM-", "");
            if (!Enum.TryParse<Station>(stationName, true, out station))
                throw new ArgumentException($"La estación '{stationName}' no es válida.");
        }

        // Obtener tickets de las últimas 24 horas para evitar líos de zona horaria
        var tickets = await ticketRepository.GetDailyTicketsAsync(request.CompanyCen, cancellationToken);
        
        var result = new List<KdsItemContractResponse>();

        foreach (var ticket in tickets)
        {
            // El KDS solo muestra items que ya fueron ENVIADOS (Preparing o Ready)
            // Si están en 'Pending', es que aún no le diste al botón de "Enviar a Cocina"
            var stationLines = ticket.Lines
                .Where(l => l.Station == station && l.Status != TicketLineStatus.Pending && l.Status != TicketLineStatus.Served)
                .ToList();

            foreach (var line in stationLines)
            {
                result.Add(new KdsItemContractResponse(
                    line.Cen,
                    ticket.Cen,
                    line.ProductCen,
                    line.ProductName,
                    (int)line.Quantity,
                    line.Status.ToString(),
                    line.Notes,
                    line.ResendCount, 
                    line.SentAt?.ToString("o") ?? ""
                ));
            }
        }

        return result;
    }
}
