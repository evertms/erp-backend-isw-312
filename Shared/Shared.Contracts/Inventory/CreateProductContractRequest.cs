namespace Shared.Contracts.Inventory;

public record CreateProductContractRequest(
    string Sku,
    string Name,
    string? Description,
    string CategoryCen,
    string UnitCen,
    double SalePrice,
    double? CostPrice,
    double ReorderLevel,
    string? StationCode
);
