namespace Shared.Contracts.Sales;

public record DailySalesDashboardDto(
    double TotalSales,
    int TicketsCount,
    double AverageTicket
);
