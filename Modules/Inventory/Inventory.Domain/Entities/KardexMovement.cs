using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class KardexMovement
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public Guid? DocumentId { get; private set; }
    
    public MovementType MovementType { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Balance { get; private set; }
    public string? Reason { get; private set; }
    public DateTime MovementDate { get; private set; }

    public virtual Product Product { get; private set; } = null!;
    public virtual Warehouse Warehouse { get; private set; } = null!;
    public virtual InventoryDocument? Document { get; private set; }

    protected KardexMovement() { }

    private KardexMovement(Guid id, Guid companyId, Guid productId, Guid warehouseId, Guid? documentId, MovementType movementType, decimal quantity, decimal balance, string? reason, DateTime movementDate)
    {
        Id = id;
        CompanyId = companyId;
        ProductId = productId;
        WarehouseId = warehouseId;
        DocumentId = documentId;
        MovementType = movementType;
        Quantity = quantity;
        Balance = balance;
        Reason = reason;
        MovementDate = movementDate;
    }

    public static KardexMovement Create(Guid companyId, Guid productId, Guid warehouseId, MovementType type, decimal quantity, decimal currentBalance, Guid? documentId = null, string? reason = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        decimal newBalance = type == MovementType.In 
            ? currentBalance + quantity 
            : currentBalance - quantity;

        if (newBalance < 0)
            throw new InvalidOperationException($"El movimiento de salida dejaría el saldo negativo. Saldo actual: {currentBalance}, Cantidad: {quantity}");

        return new KardexMovement(Guid.NewGuid(), companyId, productId, warehouseId, documentId, type, quantity, newBalance, reason, DateTime.UtcNow);
    }
}