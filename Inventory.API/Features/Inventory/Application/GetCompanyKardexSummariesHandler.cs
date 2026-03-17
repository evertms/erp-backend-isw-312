using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Inventory.Application;

public class GetCompanyKardexSummariesHandler
{
    private readonly AppDbContext _db;

    public GetCompanyKardexSummariesHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CompanyKardexSummaryDto>> HandleAsync(Guid companyId)
    {
        return await _db.Products
            .Where(p => p.CompanyId == companyId && p.Status == "Activo")
            .Select(p => new CompanyKardexSummaryDto(
                p.Id,
                p.Code ?? string.Empty,
                p.Name,
                p.ProductStocks.Sum(ps => (decimal?)ps.CurrentQuantity) ?? 0m
            ))
            .ToListAsync();
    }
}
