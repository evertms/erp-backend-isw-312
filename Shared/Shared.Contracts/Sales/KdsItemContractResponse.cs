namespace Shared.Contracts.Sales;

public record KdsItemContractResponse(
    string TicketItemCen,
    string TicketCen,
    string ProductCen,
    string ProductName,
    int Quantity,
    string Status,
    string? Note,
    int ResendCount,
    string CreatedAt
);
