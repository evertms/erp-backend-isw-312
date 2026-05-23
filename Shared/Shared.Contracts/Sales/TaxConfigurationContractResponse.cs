namespace Shared.Contracts.Sales;

public record TaxConfigurationContractResponse(
    string CompanyCen,
    double GlobalTaxPercentage
);
