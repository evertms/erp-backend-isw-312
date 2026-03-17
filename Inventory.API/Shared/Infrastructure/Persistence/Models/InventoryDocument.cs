using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class InventoryDocument
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid WarehouseId { get; set; }

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime DocumentDate { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<InventoryDocumentLine> InventoryDocumentLines { get; set; } = new List<InventoryDocumentLine>();

    public virtual ICollection<KardexMovement> KardexMovements { get; set; } = new List<KardexMovement>();

    public virtual Warehouse Warehouse { get; set; } = null!;
}
