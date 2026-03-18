namespace Inventory.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
