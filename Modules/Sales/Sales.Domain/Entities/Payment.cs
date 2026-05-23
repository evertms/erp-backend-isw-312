using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Payment
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public int TicketId { get; private set; }
    
    public PaymentMethod Method { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaidAt { get; private set; }

    public virtual Ticket Ticket { get; private set; } = null!;

    protected Payment() { }

    private Payment(string cen, int ticketId, PaymentMethod method, decimal amount, DateTime paidAt)
    {
        Cen = cen;
        TicketId = ticketId;
        Method = method;
        Amount = amount;
        PaidAt = paidAt;
    }

    internal static Payment Create(int ticketId, PaymentMethod method, decimal amount)
    {
        var cen = $"PAY-{Guid.CreateVersion7()}";
        return new Payment(cen, ticketId, method, amount, DateTime.UtcNow);
    }
}
