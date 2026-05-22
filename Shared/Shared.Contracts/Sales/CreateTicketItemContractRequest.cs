namespace Shared.Contracts.Sales;

public record CreateTicketItemContractRequest(
    string ProductCen,
    int Quantity,
    string? Note
);
