namespace Inventory.Domain.Entities;

public class Supplier
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? ContactInfo { get; private set; }
    public bool IsActive { get; private set; }

    protected Supplier() { }

    private Supplier(Guid id, Guid companyId, string name, string? contactInfo, bool isActive)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        ContactInfo = contactInfo;
        IsActive = isActive;
    }

    public static Supplier Create(Guid companyId, string name, string? contactInfo = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del proveedor no puede estar vacío.", nameof(name));

        return new Supplier(Guid.NewGuid(), companyId, name, contactInfo, true);
    }
}