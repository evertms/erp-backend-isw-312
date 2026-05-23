using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class StationCategoryConfig
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical Ref (Core)
    public string CategoryCen { get; private set; } = null!; // Logical Ref (Inventory.Categories)
    public Station Station { get; private set; }

    protected StationCategoryConfig() { }

    private StationCategoryConfig(string cen, string companyCen, string categoryCen, Station station)
    {
        Cen = cen;
        CompanyCen = companyCen;
        CategoryCen = categoryCen;
        Station = station;
    }

    public static StationCategoryConfig Create(string companyCen, string categoryCen, Station station)
    {
        var cen = $"STCFG-{Guid.CreateVersion7()}";
        return new StationCategoryConfig(cen, companyCen, categoryCen, station);
    }
}
