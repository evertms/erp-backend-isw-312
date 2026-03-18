using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; }
    public string Name { get; private set; } = null!;
    public Role Role { get; private set; }
    public bool IsActive { get; private set; }

    public virtual Company Company { get; private set; } = null!;

    // Constructor privado para EF Core
    protected User() { }

    private User(Guid id, Guid companyId, string name, Role role, bool isActive)
    {
        Id = id;
        CompanyId = companyId;
        Name = name;
        Role = role;
        IsActive = isActive;
    }

    public static User Create(Guid companyId, string name, Role role)
    {
        // Regla de Rol de Usuario: Validada implícitamente por el enum Role
        // Todo usuario registrado debe pertenecer a uno de los roles operativos predefinidos (SUPERADMIN, ADMIN, CAJERO, MESERO, COCINA, BAR).
        return new User(Guid.NewGuid(), companyId, name, role, true);
    }

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
    }
}
