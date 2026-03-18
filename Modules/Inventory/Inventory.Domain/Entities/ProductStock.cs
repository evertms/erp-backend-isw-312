namespace Inventory.Domain.Entities;

public class ProductStock
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    
    public decimal CurrentQuantity { get; private set; }
    public DateTime LastUpdated { get; private set; }

    public virtual Product Product { get; private set; } = null!;
    public virtual Warehouse Warehouse { get; private set; } = null!;

    protected ProductStock() { }

    private ProductStock(Guid id, Guid companyId, Guid productId, Guid warehouseId, decimal currentQuantity, DateTime lastUpdated)
    {
        Id = id;
        CompanyId = companyId;
        ProductId = productId;
        WarehouseId = warehouseId;
        CurrentQuantity = currentQuantity;
        LastUpdated = lastUpdated;
    }

    public static ProductStock Create(Guid companyId, Guid productId, Guid warehouseId)
    {
        return new ProductStock(Guid.NewGuid(), companyId, productId, warehouseId, 0, DateTime.UtcNow);
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
