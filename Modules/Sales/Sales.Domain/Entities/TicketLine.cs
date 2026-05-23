using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class TicketLine
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public int TicketId { get; private set; }
    
    public int? CommandNumber { get; private set; }
    public int ResendCount { get; private set; }
    public string ProductCen { get; private set; } = null!; // Logical Ref (Inventory.Products) - Immutable Replica
    public string ProductName { get; private set; } = null!; // Replica
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; } // Replica
    public string? Notes { get; private set; }
    
    public Station Station { get; private set; }
    public DateTime? SentAt { get; private set; }
    public TicketLineStatus Status { get; private set; }

    public virtual Ticket Ticket { get; private set; } = null!;

    protected TicketLine() { }

    private TicketLine(string cen, int ticketId, string productCen, string productName, decimal quantity, decimal unitPrice, Station station, string? notes)
    {
        Cen = cen;
        TicketId = ticketId;
        ProductCen = productCen;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Station = station;
        Notes = notes;
        Status = TicketLineStatus.Pending;
    }

    internal static TicketLine Create(int ticketId, string productCen, string productName, decimal quantity, decimal unitPrice, Station station, string? notes)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("El precio unitario no puede ser negativo.", nameof(unitPrice));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("El nombre del producto no puede estar vacío (Réplica requerida).", nameof(productName));

        if (string.IsNullOrWhiteSpace(productCen))
            throw new ArgumentException("El CEN del producto es obligatorio.", nameof(productCen));

        var cen = $"CMD-{Guid.CreateVersion7()}";
        return new TicketLine(cen, ticketId, productCen, productName, quantity, unitPrice, station, notes);
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

    public void Update(decimal quantity, string? notes)
    {
        if (Status != TicketLineStatus.Pending)
            throw new InvalidOperationException("Solo se pueden actualizar ítems en estado pendiente.");

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        Quantity = quantity;
        Notes = notes;
    }

    public void Resend()
    {
        if (Status == TicketLineStatus.Pending)
            throw new InvalidOperationException("No se puede reenviar un ítem pendiente.");

        ResendCount++;
        SentAt = DateTime.UtcNow;
        Status = TicketLineStatus.Preparing;
    }
}
