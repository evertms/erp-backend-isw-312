namespace Inventory.Domain.Entities;

public class Supplier
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public string Name { get; set; } = null!;
    public string? ContactInfo { get; set; }
    public bool IsActive { get; set; }
}
