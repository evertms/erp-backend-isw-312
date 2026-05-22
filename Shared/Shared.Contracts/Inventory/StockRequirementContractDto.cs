namespace Shared.Contracts.Inventory;

public record StockRequirementContractDto(
    string ProductCen,
    string ProductName,
    string WarehouseCen,
    double RequestedQuantity,
    double AvailableQuantity,
    double MissingQuantity,
    string UnitName,
    string Reason
);
