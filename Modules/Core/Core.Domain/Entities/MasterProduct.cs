namespace Core.Domain.Entities;

public class MasterProduct
{
    public Guid Id { get; private set; }
    public string Barcode { get; private set; } = null!;
    public string StandardName { get; private set; } = null!;
    public string? ImageUrl { get; private set; }

    protected MasterProduct() { }

    private MasterProduct(Guid id, string barcode, string standardName, string? imageUrl)
    {
        Id = id;
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

        return new MasterProduct(Guid.NewGuid(), barcode, standardName, imageUrl);
    }
}