using Domain.Enums;

namespace Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical Ref (Core)
    public Guid? CustomerId { get; set; } // Logical Ref (Core.Customers)
    public Guid WaiterId { get; set; } // Logical Ref (Core.Users)
    
    public TicketStatus Status { get; set; }
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TicketLine> Lines { get; set; } = new List<TicketLine>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
