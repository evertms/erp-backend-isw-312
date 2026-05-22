namespace Inventory.Domain.Entities;

public class Unit
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;

    protected Unit() { }

    private Unit(string cen, Guid companyId, string name, string code)
    {
        Cen = cen;
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

        var cen = $"UNT-{Guid.CreateVersion7()}";
        return new Unit(cen, companyId, name, code);
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
