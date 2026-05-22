namespace Sales.Domain.Entities;

public class TaxConfiguration
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical Ref (Core)
    public decimal GlobalTaxRate { get; private set; }

    protected TaxConfiguration() { }

    private TaxConfiguration(string cen, string companyCen, decimal globalTaxRate)
    {
        Cen = cen;
        CompanyCen = companyCen;
        GlobalTaxRate = globalTaxRate;
    }

    public static TaxConfiguration Create(string companyCen, decimal globalTaxRate)
    {
        if (globalTaxRate < 0)
            throw new ArgumentException("La tasa de impuesto no puede ser negativa.", nameof(globalTaxRate));

        var cen = $"TAX-{Guid.CreateVersion7()}";
        return new TaxConfiguration(cen, companyCen, globalTaxRate);
    }

    public void Update(decimal globalTaxRate)
    {
        if (globalTaxRate < 0)
            throw new ArgumentException("La tasa de impuesto no puede ser negativa.", nameof(globalTaxRate));

        GlobalTaxRate = globalTaxRate;
    }
}