namespace Inventory.Domain.Entities;

public class Unit
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;

    protected Unit() { }

    private Unit(Guid id, Guid companyId, string name, string code)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        Code = code;
    }

    public static Unit Create(Guid companyId, string name, string code)
    {
        // Regla de Integridad de Unidades: Código y nombre obligatorios.
        // La duplicidad se validará en la base de datos o en la capa de Aplicación
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código no puede estar vacío.", nameof(code));

        return new Unit(Guid.NewGuid(), companyId, name, code);
    }

    public void Update(string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("El código no puede estar vacío.", nameof(code));

        Name = name;
        Code = code;
    }
}
