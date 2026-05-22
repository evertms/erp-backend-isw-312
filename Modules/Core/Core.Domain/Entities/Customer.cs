namespace Core.Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public int CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    // Regla de Cliente Opcional: El teléfono es opcional
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual Company Company { get; private set; } = null!;

    protected Customer() { }

    private Customer(string cen, int companyId, string name, string? phone, bool isActive, DateTime createdAt)
    {
        Cen = cen;
        CompanyId = companyId;
        Name = name;
        Phone = phone;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static Customer Create(int companyId, string name, string? phone)
    {
        var cen = $"CUS-{Guid.CreateVersion7()}";
        return new Customer(cen, companyId, name, phone, true, DateTime.UtcNow);
    }
}
