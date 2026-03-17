namespace Domain.Entities;

public class ProductStock
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    
    public decimal CurrentQuantity { get; set; }
    public DateTime LastUpdated { get; set; }

    public virtual Product Product { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
}
