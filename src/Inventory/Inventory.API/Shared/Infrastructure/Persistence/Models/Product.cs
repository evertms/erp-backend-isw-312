using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid UnitId { get; set; }

    public Guid? SupplierId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public decimal MinStockAlert { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<InventoryDocumentLine> InventoryDocumentLines { get; set; } = new List<InventoryDocumentLine>();

    public virtual ICollection<KardexMovement> KardexMovements { get; set; } = new List<KardexMovement>();

    public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();

    public virtual Supplier? Supplier { get; set; }

    public virtual Unit Unit { get; set; } = null!;
}
