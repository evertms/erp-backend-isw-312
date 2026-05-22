namespace Inventory.Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    protected Category() { }

    private Category(string cen, string companyCen, string name, string? description)
    {
        Cen = cen;
        CompanyCen = companyCen;
        Name = name;
        Description = description;
    }

    public static Category Create(string companyCen, string name, string? description)
    {
        // Regla de Integridad de Categorías: Rechazar creación sin nombre
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(name));
        }

        var cen = $"CAT-{Guid.CreateVersion7()}";
        return new Category(cen, companyCen, name, description);
    }

    public void Update(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(name));
        }

        Name = name;
        Description = description;
    }
}
