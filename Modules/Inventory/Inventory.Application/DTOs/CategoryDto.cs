namespace Inventory.Application.DTOs;

public record CategoryDto(
    Guid Id,
    Guid CompanyId,
    string Name,
    string? Description
);
