using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class InventoryDocument
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public Guid WarehouseId { get; set; }
    
    public DocumentType Type { get; set; }
    public DocumentStatus Status { get; set; }
    public DateTime DocumentDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual ICollection<InventoryDocumentLine> Lines { get; set; } = new List<InventoryDocumentLine>();
}
