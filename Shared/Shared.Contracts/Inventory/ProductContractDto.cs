namespace Shared.Contracts.Inventory;

public record ProductContractDto(
    string ProductCen,
    string Sku,
    string Name,
    string? Description,
    string CategoryCen,
    string CategoryName,
    string UnitCen,
    string UnitName,
    double SalePrice,
    double? CostPrice,
    double ReorderLevel,
    string Status,
    string? StationCode
);
