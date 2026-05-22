namespace Inventory.Domain.Entities;

public class Supplier
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? ContactInfo { get; private set; }
    public bool IsActive { get; private set; }

    protected Supplier() { }

    private Supplier(string cen, Guid companyId, string name, string? contactInfo, bool isActive)
    {
        Cen = cen;
        CompanyId = companyId;
        Name = name;
        ContactInfo = contactInfo;
        IsActive = isActive;
    }

    public static Supplier Create(Guid companyId, string name, string? contactInfo = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del proveedor no puede estar vacío.", nameof(name));

        var cen = $"SUP-{Guid.CreateVersion7()}";
        return new Supplier(cen, companyId, name, contactInfo, true);
    }
}