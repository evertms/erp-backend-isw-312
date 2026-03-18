using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = null!;
    public Role Role { get; set; }
    public bool IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;
}
