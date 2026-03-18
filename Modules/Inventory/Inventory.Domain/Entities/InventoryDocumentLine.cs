namespace Inventory.Domain.Entities;

public class InventoryDocumentLine
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }

    public virtual InventoryDocument Document { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
