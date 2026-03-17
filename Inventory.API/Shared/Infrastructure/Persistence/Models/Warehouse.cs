using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class Warehouse
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public bool IsActive { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<InventoryDocument> InventoryDocuments { get; set; } = new List<InventoryDocument>();

    public virtual ICollection<KardexMovement> KardexMovements { get; set; } = new List<KardexMovement>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
}
