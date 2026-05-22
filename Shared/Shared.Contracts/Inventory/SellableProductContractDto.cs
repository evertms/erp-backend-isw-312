namespace Shared.Contracts.Inventory;

public record SellableProductContractDto(
    string ProductCen,
    string Name,
    string CategoryCen,
    string CategoryName,
    double SalePrice,
    double AvailableQuantity,
    bool IsAvailable,
    string? StationCode
);
