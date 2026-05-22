namespace Shared.Contracts.Inventory;

public record CreateProductContractResponse(
    string ProductCen,
    string Sku,
    string Name,
    string Status,
    double InitialStock
);
