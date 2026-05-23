namespace Shared.Contracts.Sales;

public record PayTicketContractResponse(
    string SaleCen,
    string TicketCen,
    string Status,
    double Subtotal,
    double TaxAmount,
    double Total,
    string? InventoryDocumentCen
);
