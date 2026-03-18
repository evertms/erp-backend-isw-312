namespace Inventory.Domain.Entities;

public class Unit
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
}
