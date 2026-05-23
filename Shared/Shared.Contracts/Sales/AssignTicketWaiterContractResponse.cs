namespace Shared.Contracts.Sales;

public record AssignTicketWaiterContractResponse(
    string TicketCen,
    string WaiterCen,
    string WaiterName
);
