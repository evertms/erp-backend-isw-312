namespace Inventory.Domain.Entities;

public class InventoryDocumentLine
{
    public Guid Id { get; private set; }
    public Guid DocumentId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal Quantity { get; private set; }

    public virtual InventoryDocument Document { get; private set; } = null!;
    public virtual Product Product { get; private set; } = null!;

    protected InventoryDocumentLine() { }

    private InventoryDocumentLine(Guid id, Guid documentId, Guid productId, decimal quantity)
    {
        Id = id;
        DocumentId = documentId;
        ProductId = productId;
        Quantity = quantity;
    }

    internal static InventoryDocumentLine Create(Guid documentId, Guid productId, decimal quantity)
    {
        return new InventoryDocumentLine(Guid.NewGuid(), documentId, productId, quantity);
    }
}
