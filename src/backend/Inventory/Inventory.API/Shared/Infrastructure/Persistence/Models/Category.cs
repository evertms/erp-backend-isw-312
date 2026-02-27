using System;
using System.Collections.Generic;

namespace Inventory.API.Shared.Infrastructure.Persistence.Models;

public partial class Category
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
