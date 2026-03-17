using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class KardexMovement
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public Guid? DocumentId { get; set; }

    public string MovementType { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal Balance { get; set; }

    public string? Reason { get; set; }

    public DateTime? MovementDate { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual InventoryDocument? Document { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
