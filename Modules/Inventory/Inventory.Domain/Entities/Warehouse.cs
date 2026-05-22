namespace Inventory.Domain.Entities;

public class Warehouse
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }

    protected Warehouse() { }

    private Warehouse(string cen, Guid companyId, string name, string? location, bool isActive)
    {
        Cen = cen;
        CompanyId = companyId;
        Name = name;
        Location = location;
        IsActive = isActive;
    }

    public static Warehouse Create(Guid companyId, string name, string? location = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del almacén no puede estar vacío.", nameof(name));

        var cen = $"WH-{Guid.CreateVersion7()}";
        return new Warehouse(cen, companyId, name, location, true);
    }
}