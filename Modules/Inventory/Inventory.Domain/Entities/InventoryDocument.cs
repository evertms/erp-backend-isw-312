using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class InventoryDocument
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public Guid WarehouseId { get; private set; }
    
    public DocumentType Type { get; private set; }
    public DocumentStatus Status { get; private set; }
    public DateTime DocumentDate { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual Warehouse Warehouse { get; private set; } = null!;
    private readonly List<InventoryDocumentLine> _lines = new();
    public virtual IReadOnlyCollection<InventoryDocumentLine> Lines => _lines.AsReadOnly();

    protected InventoryDocument() { }

    private InventoryDocument(Guid id, Guid companyId, Guid warehouseId, DocumentType type, DocumentStatus status, DateTime documentDate, string? notes, DateTime createdAt)
    {
        Id = id;
        CompanyId = companyId;
        WarehouseId = warehouseId;
        Type = type;
        Status = status;
        DocumentDate = documentDate;
        Notes = notes;
        CreatedAt = createdAt;
    }

    public static InventoryDocument Create(Guid companyId, Guid warehouseId, DocumentType type, DateTime documentDate, string? notes = null)
    {
        // Regla de Auditoría de Ajustes: Obligatorio motivo si es ajuste (u otra operación directa)
        if (type == DocumentType.Ajuste && string.IsNullOrWhiteSpace(notes))
        {
            throw new ArgumentException("Debe proporcionar un motivo (notas) para los documentos de tipo Ajuste.", nameof(notes));
        }

        return new InventoryDocument(Guid.NewGuid(), companyId, warehouseId, type, DocumentStatus.Borrador, documentDate, notes, DateTime.UtcNow);
    }

    public void AddLine(Guid productId, decimal quantity)
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
