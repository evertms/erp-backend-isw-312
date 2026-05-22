using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class KardexMovement
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public int ProductId { get; private set; }
    public int WarehouseId { get; private set; }
    public int? DocumentId { get; private set; }
    
    public MovementType MovementType { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Balance { get; private set; }
    public string? Reason { get; private set; }
    public DateTime MovementDate { get; private set; }

    public virtual Product Product { get; private set; } = null!;
    public virtual Warehouse Warehouse { get; private set; } = null!;
    public virtual InventoryDocument? Document { get; private set; }

    protected KardexMovement() { }

    private KardexMovement(string cen, string companyCen, int productId, int warehouseId, int? documentId, MovementType movementType, decimal quantity, decimal balance, string? reason, DateTime movementDate)
    {
        Cen = cen;
        CompanyCen = companyCen;
        ProductId = productId;
        WarehouseId = warehouseId;
        DocumentId = documentId;
        MovementType = movementType;
        Quantity = quantity;
        Balance = balance;
        Reason = reason;
        MovementDate = movementDate;
    }

    public static KardexMovement Create(string companyCen, int productId, int warehouseId, MovementType type, decimal quantity, decimal currentBalance, int? documentId = null, string? reason = null)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a 0.", nameof(quantity));

        decimal newBalance = type == MovementType.In 
            ? currentBalance + quantity 
            : currentBalance - quantity;

        if (newBalance < 0)
            throw new InvalidOperationException($"El movimiento de salida dejaría el saldo negativo. Saldo actual: {currentBalance}, Cantidad: {quantity}");

        var cen = $"MOV-{Guid.CreateVersion7()}";
        return new KardexMovement(cen, companyCen, productId, warehouseId, documentId, type, quantity, newBalance, reason, DateTime.UtcNow);
    }
}