using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
