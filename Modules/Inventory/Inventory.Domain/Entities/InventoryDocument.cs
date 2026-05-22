using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class InventoryDocument
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public int WarehouseId { get; private set; }
    
    public DocumentType Type { get; private set; }
    public DocumentStatus Status { get; private set; }
    public DateTime DocumentDate { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual Warehouse Warehouse { get; private set; } = null!;
    private readonly List<InventoryDocumentLine> _lines = new();
    public virtual IReadOnlyCollection<InventoryDocumentLine> Lines => _lines.AsReadOnly();

    protected InventoryDocument() { }

    private InventoryDocument(string cen, string companyCen, int warehouseId, DocumentType type, DocumentStatus status, DateTime documentDate, string? notes, DateTime createdAt)
    {
        Cen = cen;
        CompanyCen = companyCen;
        WarehouseId = warehouseId;
        Type = type;
        Status = status;
        DocumentDate = documentDate;
        Notes = notes;
        CreatedAt = createdAt;
    }

    public static InventoryDocument Create(string companyCen, int warehouseId, DocumentType type, DateTime documentDate, string? notes = null)
    {
        // Regla de Auditoría de Ajustes: Obligatorio motivo si es ajuste (u otra operación directa)
        if (type == DocumentType.Ajuste && string.IsNullOrWhiteSpace(notes))
        {
            throw new ArgumentException("Debe proporcionar un motivo (notas) para los documentos de tipo Ajuste.", nameof(notes));
        }

        var cen = $"DOC-{Guid.CreateVersion7()}";
        return new InventoryDocument(cen, companyCen, warehouseId, type, DocumentStatus.Borrador, documentDate, notes, DateTime.UtcNow);
    }

    public void AddLine(int productId, decimal quantity)
    {
        if (Status != DocumentStatus.Borrador)
            throw new InvalidOperationException("Solo se pueden agregar líneas a un documento en estado Borrador.");

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        var line = InventoryDocumentLine.Create(Id, productId, quantity);
        _lines.Add(line);
    }

    public void Confirm()
    {
        if (Status != DocumentStatus.Borrador)
            throw new InvalidOperationException("El documento solo puede ser confirmado si está en estado Borrador.");
            
        Status = DocumentStatus.Confirmado;
    }
}
