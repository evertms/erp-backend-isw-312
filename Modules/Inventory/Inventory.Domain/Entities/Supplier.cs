namespace Inventory.Domain.Entities;

public class Supplier
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? ContactInfo { get; private set; }
    public bool IsActive { get; private set; }

    protected Supplier() { }

    private Supplier(string cen, string companyCen, string name, string? contactInfo, bool isActive)
    {
        Cen = cen;
        CompanyCen = companyCen;
        Name = name;
        ContactInfo = contactInfo;
        IsActive = isActive;
    }

    public static Supplier Create(string companyCen, string name, string? contactInfo = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del proveedor no puede estar vacío.", nameof(name));

        var cen = $"SUP-{Guid.CreateVersion7()}";
        return new Supplier(cen, companyCen, name, contactInfo, true);
    }
}