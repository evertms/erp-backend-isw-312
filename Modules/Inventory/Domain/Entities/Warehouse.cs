namespace Domain.Entities;

public class Warehouse
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public string Name { get; set; } = null!;
    public string? Location { get; set; }
    public bool IsActive { get; set; }
}
