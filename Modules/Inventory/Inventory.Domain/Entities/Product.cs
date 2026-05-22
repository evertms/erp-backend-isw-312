using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string CompanyCen { get; private set; } = null!; // Logical ref to Core
    public int CategoryId { get; private set; }
    public int UnitId { get; private set; }
    public int? SupplierId { get; private set; }
    
    public string? Code { get; private set; }
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public ProductStatus Status { get; private set; }
    public string? ImageUrl { get; private set; }
    public decimal MinStockAlert { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual Category Category { get; private set; } = null!;
    public virtual Unit Unit { get; private set; } = null!;
    public virtual Supplier? Supplier { get; private set; }

    protected Product() { }

    private Product(string cen, string companyCen, int categoryId, int unitId, int? supplierId, string? code, string name, decimal price, ProductStatus status, string? imageUrl, decimal minStockAlert, DateTime createdAt)
    {
        Cen = cen;
        CompanyCen = companyCen;
        CategoryId = categoryId;
        UnitId = unitId;
        SupplierId = supplierId;
        Code = code;
        Name = name;
        Price = price;
        Status = status;
        ImageUrl = imageUrl;
        MinStockAlert = minStockAlert;
        CreatedAt = createdAt;
    }

    public static Product Create(string companyCen, int categoryId, int unitId, string name, decimal price, string? code = null, int? supplierId = null, string? imageUrl = null, decimal minStockAlert = 0)
    {
        Validate(name, categoryId, unitId, price);
        var cen = $"PROD-{Guid.CreateVersion7()}";
        return new Product(cen, companyCen, categoryId, unitId, supplierId, code, name, price, ProductStatus.Activo, imageUrl, minStockAlert, DateTime.UtcNow);
    }

    public void Update(string name, int categoryId, int unitId, decimal price, string? code, int? supplierId, string? imageUrl, decimal minStockAlert)
    {
        Validate(name, categoryId, unitId, price);
        
        Name = name;
        CategoryId = categoryId;
        UnitId = unitId;
        Price = price;
        Code = code;
        SupplierId = supplierId;
        ImageUrl = imageUrl;
        MinStockAlert = minStockAlert;
    }

    public void UpdateStatus(ProductStatus status)
    {
        Status = status;
    }

    public void CheckCanBeSold()
    {
        if (Status == ProductStatus.Inactivo || Status == ProductStatus.Agotado)
        {
            throw new InvalidOperationException($"El producto '{Name}' no puede ser comercializado porque está en estado '{Status}'.");
        }
    }
    
    private static void Validate(string name, int categoryId, int unitId, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        
        if (categoryId <= 0)
            throw new ArgumentException("La categoría es obligatoria.", nameof(categoryId));
        
        if (unitId <= 0)
            throw new ArgumentException("La unidad de medida es obligatoria.", nameof(unitId));
            
        if (price <= 0)
            throw new ArgumentException("El precio debe ser estrictamente mayor a 0.", nameof(price));
    }
}
