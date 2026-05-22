namespace Core.Domain.Entities;

public class MasterProduct
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string Barcode { get; private set; } = null!;
    public string StandardName { get; private set; } = null!;
    public string? ImageUrl { get; private set; }

    protected MasterProduct() { }

    private MasterProduct(string cen, string barcode, string standardName, string? imageUrl)
    {
        Cen = cen;
        Barcode = barcode;
        StandardName = standardName;
        ImageUrl = imageUrl;
    }

    public static MasterProduct Create(string barcode, string standardName, string? imageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            throw new ArgumentException("El código de barras no puede estar vacío.", nameof(barcode));

        if (string.IsNullOrWhiteSpace(standardName))
            throw new ArgumentException("El nombre estándar no puede estar vacío.", nameof(standardName));

        var cen = $"MP-{Guid.CreateVersion7()}";
        return new MasterProduct(cen, barcode, standardName, imageUrl);
    }
}