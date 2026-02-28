namespace Inventory.API.Features.Inventory.Application;

public record CompanyKardexSummaryDto(
    Guid ProductId,
    string Code,
    string Name,
    decimal TotalStock
);
