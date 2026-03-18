using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public Guid CategoryId { get; set; }
    public Guid UnitId { get; set; }
    public Guid? SupplierId { get; set; }
    
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public string? ImageUrl { get; set; }
    public decimal MinStockAlert { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;
    public virtual Unit Unit { get; set; } = null!;
    public virtual Supplier? Supplier { get; set; }
}
