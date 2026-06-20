namespace Sales.Domain.Entities;

public class SalesConfiguration
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical Ref (Core)
    public string DefaultWarehouseCen { get; private set; } = null!; // Logical Ref (Inventory)

    protected SalesConfiguration() { }

    private SalesConfiguration(string cen, string companyCen, string defaultWarehouseCen)
    {
        Cen = cen;
        CompanyCen = companyCen;
        DefaultWarehouseCen = defaultWarehouseCen;
    }

    public static SalesConfiguration Create(string companyCen, string defaultWarehouseCen)
    {
        if (string.IsNullOrWhiteSpace(companyCen)) throw new ArgumentException("Required", nameof(companyCen));
        if (string.IsNullOrWhiteSpace(defaultWarehouseCen)) throw new ArgumentException("Required", nameof(defaultWarehouseCen));

        var cen = $"SALESCFG-{Guid.CreateVersion7()}";
        return new SalesConfiguration(cen, companyCen, defaultWarehouseCen);
    }

    public void UpdateDefaultWarehouse(string defaultWarehouseCen)
    {
        if (string.IsNullOrWhiteSpace(defaultWarehouseCen)) throw new ArgumentException("Required", nameof(defaultWarehouseCen));
        DefaultWarehouseCen = defaultWarehouseCen;
    }
}
