using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; }
    public Guid TicketId { get; private set; }
    
    public PaymentMethod Method { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaidAt { get; private set; }

    public virtual Ticket Ticket { get; private set; } = null!;

    protected Payment() { }

    private Payment(Guid id, Guid ticketId, PaymentMethod method, decimal amount, DateTime paidAt)
    {
        Id = id;
        TicketId = ticketId;
        Method = method;
        Amount = amount;
        PaidAt = paidAt;
    }

    internal static Payment Create(Guid ticketId, PaymentMethod method, decimal amount)
    {
        return new Payment(Guid.NewGuid(), ticketId, method, amount, DateTime.UtcNow);
    }
}
