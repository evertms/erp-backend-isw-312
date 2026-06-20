namespace Inventory.Domain.Entities;

public class Warehouse
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? Location { get; private set; }
    public bool IsActive { get; private set; }

    protected Warehouse() { }

    private Warehouse(string cen, string companyCen, string name, string? location, bool isActive)
    {
        Cen = cen;
        CompanyCen = companyCen;
        Name = name;
        Location = location;
        IsActive = isActive;
    }

    public static Warehouse Create(string companyCen, string name, string? location = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del almacén no puede estar vacío.", nameof(name));

        var cen = $"WH-{Guid.CreateVersion7()}";
        return new Warehouse(cen, companyCen, name, location, true);
    }

    public static Warehouse CreateWithCen(string cen, string companyCen, string name, string? location = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del almacén no puede estar vacío.", nameof(name));
        if (string.IsNullOrWhiteSpace(cen))
            throw new ArgumentException("El CEN no puede estar vacío.", nameof(cen));

        return new Warehouse(cen, companyCen, name, location, true);
    }
}