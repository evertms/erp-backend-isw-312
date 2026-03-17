using Domain.Enums;

namespace Domain.Entities;

public class TicketLine
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    
    public int CommandNumber { get; set; }
    public Guid ProductId { get; set; } // Logical Ref (Inventory.Products)
    public string ProductName { get; set; } = null!; // Replica
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Notes { get; set; }
    
    public Station Station { get; set; }
    public DateTime? SentAt { get; set; }
    public TicketLineStatus Status { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
