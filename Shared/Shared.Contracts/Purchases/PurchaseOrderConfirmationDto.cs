namespace Shared.Contracts.Purchases;

public class PurchaseOrderConfirmationDto
{
    public required string OrderCen { get; set; }
    public required PurchaseStatus Status { get; set; }
    public required DateTime ConfirmedAt { get; set; }
}
