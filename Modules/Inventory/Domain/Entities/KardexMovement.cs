using Domain.Enums;

namespace Domain.Entities;

public class KardexMovement
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical ref to Core
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid? DocumentId { get; set; }
    
    public MovementType MovementType { get; set; }
    public decimal Quantity { get; set; }
    public decimal Balance { get; set; }
    public string? Reason { get; set; }
    public DateTime MovementDate { get; set; }

    public virtual Product Product { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
    public virtual InventoryDocument? Document { get; set; }
}
