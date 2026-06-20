namespace Shared.Contracts.Purchases;

public class PurchaseOrderListDto
{
    public required string OrderCen { get; set; }
    public required PurchaseStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public required string SupplierCen { get; set; }
    public required int ItemCount { get; set; }
}
