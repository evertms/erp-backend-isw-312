namespace Shared.Contracts.Sales;

public record TicketItemContractResponse(
    string TicketItemCen,
    string ProductCen,
    string ProductName,
    int Quantity,
    double UnitPrice,
    string? Note,
    string Status,
    string? SentAt,
    int ResendCount
);
