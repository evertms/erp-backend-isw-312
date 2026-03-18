namespace Core.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    // Regla de Cliente Opcional: El teléfono es opcional
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual Company Company { get; private set; } = null!;

    protected Customer() { }

    private Customer(Guid id, Guid companyId, string name, string? phone, bool isActive, DateTime createdAt)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        Phone = phone;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static Customer Create(Guid companyId, string name, string? phone)
    {
        return new Customer(Guid.NewGuid(), companyId, name, phone, true, DateTime.UtcNow);
    }
}
