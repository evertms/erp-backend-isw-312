using MediatR;
using Sales.Domain.Enums;
using Sales.Domain.Repositories;
using Shared.Contracts.Sales;

namespace Sales.Application.Features.Kds.Queries.GetItems;

public record GetKdsItemsQuery(string CompanyCen, string TeamCen) : IRequest<List<KdsItemContractResponse>>;

public class GetKdsItemsHandler(ITicketRepository ticketRepository) : IRequestHandler<GetKdsItemsQuery, List<KdsItemContractResponse>>
{
    public async Task<List<KdsItemContractResponse>> Handle(GetKdsItemsQuery request, CancellationToken cancellationToken)
    {
        // Parse station from TeamCen (e.g. "TEAM-Cocina")
        var stationName = request.TeamCen.Replace("TEAM-", "");
        if (!Enum.TryParse<Station>(stationName, true, out var station))
            throw new ArgumentException($"La estación '{stationName}' no es válida.");

        var tickets = await ticketRepository.GetDailyTicketsAsync(request.CompanyCen, cancellationToken);
        
        var result = new List<KdsItemContractResponse>();

        foreach (var ticket in tickets)
        {
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
                    0, // ResendCount placeholder
                    line.SentAt?.ToString("o") ?? ""
                ));
            }
        }

        return result;
    }
}
