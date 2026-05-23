namespace Shared.Contracts.Sales;

public record TopProductDashboardContractResponse(
    string? ProductCen,
    string ProductName,
    int TotalQuantity,
    string? CategoryCen,
    string? CategoryName,
    double SalePrice
);
