namespace Inventory.Domain.Entities;

public class Unit
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    protected Unit() { }

    private Unit(Guid id, Guid companyId, string code, string name)
    {
        Id = id;
        CompanyId = companyId;
        Code = code;
        Name = name;
    }

    public static Unit Create(Guid companyId, string code, string name)
    {
        // Regla de Integridad de Unidades: Código y nombre obligatorios.
        // La duplicidad se validará en la base de datos o en la capa de Aplicación
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código no puede estar vacío.", nameof(code));
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));

        return new Unit(Guid.NewGuid(), companyId, code, name);
    }
}
