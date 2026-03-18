using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class StationCategoryConfig
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical Ref (Core)
    public Guid CategoryId { get; private set; } // Logical Ref (Inventory.Categories)
    public Station Station { get; private set; }

    protected StationCategoryConfig() { }

    private StationCategoryConfig(Guid id, Guid companyId, Guid categoryId, Station station)
    {
        Id = id;
        CompanyId = companyId;
        CategoryId = categoryId;
        Station = station;
    }

    public static StationCategoryConfig Create(Guid companyId, Guid categoryId, Station station)
    {
        return new StationCategoryConfig(Guid.NewGuid(), companyId, categoryId, station);
    }
}
