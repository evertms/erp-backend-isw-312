namespace Inventory.Domain.Entities;

public class ProductStock
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public int ProductId { get; private set; }
    public int WarehouseId { get; private set; }
    
    public decimal CurrentQuantity { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public virtual Product Product { get; private set; } = null!;
    public virtual Warehouse Warehouse { get; private set; } = null!;

    protected ProductStock() { }

    private ProductStock(string cen, string companyCen, int productId, int warehouseId, decimal currentQuantity, DateTime lastUpdated)
    {
        Cen = cen;
        CompanyCen = companyCen;
        ProductId = productId;
        WarehouseId = warehouseId;
        CurrentQuantity = currentQuantity;
        LastUpdated = lastUpdated;
    }

    public static ProductStock Create(string companyCen, int productId, int warehouseId)
    {
        var cen = $"STK-{Guid.CreateVersion7()}";
        return new ProductStock(cen, companyCen, productId, warehouseId, 0, DateTime.UtcNow);
    }

    public void AddQuantity(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad a añadir debe ser mayor a 0.", nameof(quantity));

        CurrentQuantity += quantity;
        LastUpdated = DateTime.UtcNow;
    }

    public void SubtractQuantity(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad a restar debe ser mayor a 0.", nameof(quantity));

        // Regla de Invariante Absoluta de Stock: No puede ser negativo
        if (CurrentQuantity - quantity < 0)
        {
            throw new InvalidOperationException($"Stock insuficiente. No se pueden descontar {quantity} unidades. Stock actual: {CurrentQuantity}");
        }

        CurrentQuantity -= quantity;
        LastUpdated = DateTime.UtcNow;
    }
}
