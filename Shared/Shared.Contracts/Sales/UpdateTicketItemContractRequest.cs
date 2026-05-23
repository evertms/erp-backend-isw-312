namespace Shared.Contracts.Sales;

public record UpdateTicketItemContractRequest(
    int Quantity,
    string? Note
);
