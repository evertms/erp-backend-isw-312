using Domain.Enums;

namespace Domain.Entities;

public class StationCategoryConfig
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical Ref (Core)
    public Guid CategoryId { get; set; } // Logical Ref (Inventory.Categories)
    public Station Station { get; set; }
}
