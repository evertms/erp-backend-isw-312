namespace Sales.Domain.Entities;

public class TaxConfiguration
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical Ref (Core)
    public decimal GlobalTaxRate { get; private set; }

    protected TaxConfiguration() { }

    private TaxConfiguration(Guid id, Guid companyId, decimal globalTaxRate)
    {
        Id = id;
        CompanyId = companyId;
        GlobalTaxRate = globalTaxRate;
    }

    public static TaxConfiguration Create(Guid companyId, decimal globalTaxRate)
    {
        if (globalTaxRate < 0)
            throw new ArgumentException("La tasa de impuesto no puede ser negativa.", nameof(globalTaxRate));

        return new TaxConfiguration(Guid.NewGuid(), companyId, globalTaxRate);
    }
}