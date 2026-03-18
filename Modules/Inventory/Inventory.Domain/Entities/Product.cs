using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public Guid CompanyId { get; private set; } // Logical ref to Core
    public Guid CategoryId { get; private set; }
    public Guid UnitId { get; private set; }
    public Guid? SupplierId { get; private set; }
    
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

    private Product(Guid id, Guid companyId, Guid categoryId, Guid unitId, Guid? supplierId, string? code, string name, decimal price, ProductStatus status, string? imageUrl, decimal minStockAlert, DateTime createdAt)
    {
        Id = id;
        CompanyId = companyId;
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

    public static Product Create(Guid companyId, Guid categoryId, Guid unitId, string name, decimal price, string? code = null, Guid? supplierId = null, string? imageUrl = null, decimal minStockAlert = 0)
    {
        // Regla de Composición de Producto: No puede instanciarse sin nombre, precio, categoría y unidad de medida
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        
        if (categoryId == Guid.Empty)
            throw new ArgumentException("La categoría es obligatoria.", nameof(categoryId));
        
        if (unitId == Guid.Empty)
            throw new ArgumentException("La unidad de medida es obligatoria.", nameof(unitId));
            
        // Regla de Invariante de Precio: Estrictamente mayor a 0
        if (price <= 0)
            throw new ArgumentException("El precio debe ser estrictamente mayor a 0.", nameof(price));

        return new Product(Guid.NewGuid(), companyId, categoryId, unitId, supplierId, code, name, price, ProductStatus.Activo, imageUrl, minStockAlert, DateTime.UtcNow);
    }

    public void CheckCanBeSold()
    {
        // Regla de Bloqueo por Estado: Producto Inactivo o Agotado no puede ser comercializado
        if (Status == ProductStatus.Inactivo || Status == ProductStatus.Agotado)
        {
            throw new InvalidOperationException($"El producto '{Name}' no puede ser comercializado porque está en estado '{Status}'.");
        }
    }

    public void UpdateStatus(ProductStatus status)
    {
        Status = status;
    }
}
