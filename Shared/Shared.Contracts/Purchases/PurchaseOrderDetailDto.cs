namespace Shared.Contracts.Purchases;

public class PurchaseOrderDetailDto
{
    public required string OrderCen { get; set; }
    public required PurchaseStatus Status { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public required string SupplierCen { get; set; }
    public required string WarehouseCen { get; set; }
    public required List<PurchaseOrderDetailItemDto> Items { get; set; } = new();
}
