namespace Shared.Contracts.Sales;

public record PaymentMethodContractResponse(
    string PaymentMethodCode,
    string Name,
    bool IsActive
);
