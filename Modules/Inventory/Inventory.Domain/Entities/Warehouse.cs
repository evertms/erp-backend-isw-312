namespace Inventory.Domain.Entities;

public class Warehouse
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }

    protected Warehouse() { }

    private Warehouse(Guid id, Guid companyId, string name, string? location, bool isActive)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        Location = location;
        IsActive = isActive;
    }

    public static Warehouse Create(Guid companyId, string name, string? location = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del almacén no puede estar vacío.", nameof(name));

        return new Warehouse(Guid.NewGuid(), companyId, name, location, true);
    }
}