namespace Shared.Contracts.Sales;

public record KdsStatusDashboardDto(
    int PendingCount,
    int PreparingCount,
    int ReadyCount
);
