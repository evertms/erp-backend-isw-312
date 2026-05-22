using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Ticket
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical Ref (Core)
    public string? CustomerCen { get; private set; } // Logical Ref (Core.Customers)
    public string WaiterCen { get; private set; } = null!; // Logical Ref (Core.Users)
    
    public int DailyNumber { get; private set; }
    public TicketStatus Status { get; private set; }
    public decimal Subtotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal Total { get; private set; }
    
    // Regla de Impuesto Snapshot: La tasa se guarda al momento de crear para evitar cambios
    public decimal AppliedTaxRate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<TicketLine> _lines = new();
    public virtual IReadOnlyCollection<TicketLine> Lines => _lines.AsReadOnly();
    
    private readonly List<Payment> _payments = new();
    public virtual IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    protected Ticket() { }

    private Ticket(string cen, string companyCen, string? customerCen, string waiterCen, decimal appliedTaxRate, int dailyNumber)
    {
        Cen = cen;
        CompanyCen = companyCen;
        CustomerCen = customerCen;
        WaiterCen = waiterCen;
        DailyNumber = dailyNumber;
        Status = TicketStatus.Open;
        AppliedTaxRate = appliedTaxRate;
        CreatedAt = DateTime.UtcNow;
    }

    public static Ticket Create(string companyCen, string waiterCen, decimal currentTaxRate, int dailyNumber, string? customerCen = null)
    {
        if (string.IsNullOrWhiteSpace(waiterCen))
            throw new ArgumentException("Es obligatorio asignar un mesero al ticket.", nameof(waiterCen));

        var cen = $"TICK-{Guid.CreateVersion7()}";
        return new Ticket(cen, companyCen, customerCen, waiterCen, currentTaxRate, dailyNumber);
    }

    public void AddLine(string productCen, string productName, decimal quantity, decimal unitPrice, Station station, string? notes = null)
    {
        // Regla de Inmutabilidad del Ticket Pagado
        if (Status == TicketStatus.Paid)
            throw new InvalidOperationException("No se pueden agregar líneas a un ticket pagado.");
            
        if (Status == TicketStatus.Canceled)
            throw new InvalidOperationException("No se pueden agregar líneas a un ticket cancelado.");

        var line = TicketLine.Create(Id, productCen, productName, quantity, unitPrice, station, notes);
        _lines.Add(line);
        RecalculateTotals();
    }

    public void AssignWaiter(string waiterCen)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("Solo se puede asignar mesero a tickets abiertos.");

        if (string.IsNullOrWhiteSpace(waiterCen))
            throw new ArgumentException("El mesero es obligatorio.", nameof(waiterCen));

        WaiterCen = waiterCen;
    }

    
    public void DispatchPendingLinesToKitchen(int commandNumber)
    {
        // Regla de Agrupación de Comandas: Solo despachar los nuevos
        var pendingLines = _lines.Where(l => l.Status == TicketLineStatus.Pending).ToList();
        
        foreach (var line in pendingLines)
        {
            line.Dispatch(commandNumber);
        }
    }

    public void Pay(PaymentMethod method, decimal amount)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("El ticket no está abierto para recibir pagos.");
            
        if (string.IsNullOrWhiteSpace(WaiterCen))
            throw new InvalidOperationException("El ticket debe tener un mesero asignado antes de cobrar.");

        // Regla de Cobro Total y Métodos: Pago único por la totalidad
        if (amount != Total)
            throw new InvalidOperationException($"El pago debe ser por el total exacto de {Total}. Monto enviado: {amount}");

        var payment = Payment.Create(Id, method, amount);
        _payments.Add(payment);
        
        Status = TicketStatus.Paid;
    }

    public void Cancel()
    {
        // Regla de Inmutabilidad del Ticket Pagado
        if (Status == TicketStatus.Paid)
            throw new InvalidOperationException("Un ticket pagado no puede ser cancelado.");
            
        Status = TicketStatus.Canceled;
    }

    private void RecalculateTotals()
    {
        Subtotal = _lines.Sum(l => l.Quantity * l.UnitPrice);
        TaxAmount = Subtotal * AppliedTaxRate;
        Total = Subtotal + TaxAmount;
    }
}
