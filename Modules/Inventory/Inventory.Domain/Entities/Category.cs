namespace Inventory.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    protected Category() { }

    private Category(Guid id, Guid companyId, string name, string? description)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        Description = description;
    }

    public static Category Create(Guid companyId, string name, string? description)
    {
        // Regla de Integridad de Categorías: Rechazar creación sin nombre
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre de la categoría no puede estar vacío.", nameof(name));
        }

        return new Category(Guid.NewGuid(), companyId, name, description);
    }
}
