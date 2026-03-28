using Inventory.Domain.Enums;

namespace Inventory.Application.DTOs;

public record ProductDetailsDto(
    Guid Id,
    Guid CompanyId,
    Guid CategoryId,
    Guid UnitId,
    Guid? SupplierId,
    string? Code,
    string Name,
    decimal Price,
    ProductStatus Status,
    string? ImageUrl,
    decimal MinStockAlert,
    DateTime CreatedAt
);
