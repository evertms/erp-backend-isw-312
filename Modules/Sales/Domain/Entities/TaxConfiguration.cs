namespace Domain.Entities;

public class TaxConfiguration
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; } // Logical Ref (Core)
    public decimal GlobalTaxRate { get; set; }
}
