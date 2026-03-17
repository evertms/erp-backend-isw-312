namespace Domain.Entities;

public class MasterProduct
{
    public Guid Id { get; set; }
    public string Barcode { get; set; } = null!;
    public string StandardName { get; set; } = null!;
    public string? ImageUrl { get; set; }
}
