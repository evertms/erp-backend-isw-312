namespace Shared.Contracts.Sales;

public record TicketTotalsContractResponse(
    string TicketCen,
    double Subtotal,
    double TaxAmount,
    double Total
);
