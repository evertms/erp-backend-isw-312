namespace Inventory.Application.DTOs;

public record UnitDto(
    Guid Id,
    Guid CompanyId,
    string Name,
    string Code
);
