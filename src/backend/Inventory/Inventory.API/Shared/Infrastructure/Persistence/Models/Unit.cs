using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class Unit
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
