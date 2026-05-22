namespace Inventory.Domain.Entities;

public class InventoryDocumentLine
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public int DocumentId { get; private set; }
    public int ProductId { get; private set; }
    public decimal Quantity { get; private set; }

    public virtual InventoryDocument Document { get; private set; } = null!;
    public virtual Product Product { get; private set; } = null!;

    protected InventoryDocumentLine() { }

    private InventoryDocumentLine(string cen, int documentId, int productId, decimal quantity)
    {
        Cen = cen;
        DocumentId = documentId;
        ProductId = productId;
        Quantity = quantity;
    }

    internal static InventoryDocumentLine Create(int documentId, int productId, decimal quantity)
    {
        var cen = $"LINE-{Guid.CreateVersion7()}";
        return new InventoryDocumentLine(cen, documentId, productId, quantity);
    }
}
