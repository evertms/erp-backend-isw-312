namespace Shared.Contracts.Sales;

public record TicketContractResponse(
    string TicketCen,
    int DailyNumber,
    string Status,
    string CreatedAt,
    string? WaiterCen,
    string? CompanyCen,
    double TaxAmount
);
