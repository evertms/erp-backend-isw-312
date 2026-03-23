namespace Inventory.Application.Features.Products.DTOs;

public record ProductDto(Guid Id, string Name, string? Code, string? ImageUrl);
