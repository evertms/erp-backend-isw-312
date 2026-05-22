namespace Shared.Contracts.Sales;

public record ProcessRestaurantOrderPaymentResultDto(
    bool IsSuccess,
    int? SaleId,
    string? SaleCen,
    string? InventoryDocumentCen,
    double Subtotal,
    double TaxAmount,
    double Total,
    string Message,
    List<StockInsufficiencyResponseDto> Insufficiencies
);
