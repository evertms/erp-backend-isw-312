using Purchases.Domain.Enums;

namespace Purchases.Domain.Entities;

public class PurchaseOrder
{
    public int Id { get; set; }
    public string Cen { get; set; } = string.Empty;
    public string CompanyCen { get; set; } = string.Empty;
    public string SupplierCen { get; set; } = string.Empty;
    public string WarehouseCen { get; set; } = string.Empty;
    public PurchaseStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }

    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
