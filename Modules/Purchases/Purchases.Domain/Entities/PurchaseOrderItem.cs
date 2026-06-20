namespace Purchases.Domain.Entities;

public class PurchaseOrderItem
{
    public int Id { get; set; }
    public string Cen { get; set; } = string.Empty;
    public int PurchaseOrderId { get; set; }
    public string ProductCen { get; set; } = string.Empty;
    public int Quantity { get; set; }

    public PurchaseOrder PurchaseOrder { get; set; } = null!;
}
