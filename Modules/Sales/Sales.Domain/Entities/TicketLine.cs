using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class TicketLine
{
    public Guid Id { get; private set; }
    public Guid TicketId { get; private set; }
    
    public int? CommandNumber { get; private set; }
    public Guid ProductId { get; private set; } // Logical Ref (Inventory.Products)
    public string ProductName { get; private set; } = null!; // Replica
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Notes { get; private set; }
    
    public Station Station { get; private set; }
    public DateTime? SentAt { get; private set; }
    public TicketLineStatus Status { get; private set; }

    public virtual Ticket Ticket { get; private set; } = null!;

    protected TicketLine() { }

    private TicketLine(Guid id, Guid ticketId, Guid productId, string productName, decimal quantity, decimal unitPrice, Station station, string? notes)
    {
        Id = id;
        TicketId = ticketId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Station = station;
        Notes = notes;
        Status = TicketLineStatus.Pending;
    }

    internal static TicketLine Create(Guid ticketId, Guid productId, string productName, decimal quantity, decimal unitPrice, Station station, string? notes)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(unitPrice));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("El nombre del producto no puede estar vacío (Réplica requerida).", nameof(productName));

        return new TicketLine(Guid.NewGuid(), ticketId, productId, productName, quantity, unitPrice, station, notes);
    }

    internal void Dispatch(int commandNumber)
    {
        if (Status != TicketLineStatus.Pending)
            throw new InvalidOperationException("Solo los ítems pendientes pueden ser enviados a preparación.");

        CommandNumber = commandNumber;
        SentAt = DateTime.UtcNow;
        Status = TicketLineStatus.Preparing;
    }

    public void MarkAsReady()
    {
        // Regla de Máquina de Estados de Preparación: Secuencial
        if (Status != TicketLineStatus.Preparing)
            throw new InvalidOperationException("El ítem debe estar en preparación para marcarse como listo.");
            
        Status = TicketLineStatus.Ready;
    }

    public void MarkAsServed()
    {
        if (Status != TicketLineStatus.Ready)
            throw new InvalidOperationException("El ítem debe estar listo para marcarse como servido.");
            
        Status = TicketLineStatus.Served;
    }
}
