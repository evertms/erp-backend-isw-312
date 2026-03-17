namespace Inventory.API.Features.Companies.Application;

public record ProductDto(Guid Id, string Name, string Code, string? ImageUrl);
