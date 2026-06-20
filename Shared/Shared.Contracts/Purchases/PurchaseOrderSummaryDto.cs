namespace Shared.Contracts.Purchases;

public class PurchaseOrderSummaryDto
{
    public required string OrderCen { get; set; }
    public required PurchaseStatus Status { get; set; }
}
