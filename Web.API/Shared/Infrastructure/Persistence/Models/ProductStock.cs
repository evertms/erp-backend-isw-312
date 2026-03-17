using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class ProductStock
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid ProductId { get; set; }

    public Guid WarehouseId { get; set; }

    public decimal CurrentQuantity { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
