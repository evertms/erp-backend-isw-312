using System;

namespace Inventory.Application.Features.Kardex.DTOs;

public record CompanyKardexSummaryDto(Guid ProductId, string Code, string Name, decimal TotalStock);
