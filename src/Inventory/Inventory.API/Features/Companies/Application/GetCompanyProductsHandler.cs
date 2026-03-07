using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Companies.Application;

public class GetCompanyProductsHandler
{
    private readonly AppDbContext _db;

    public GetCompanyProductsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductDto>> HandleAsync(Guid companyId)
    {
        return await _db.Products
            .Where(p => p.CompanyId == companyId && p.Status == "Activo")
            .Select(p => new ProductDto(p.Id, p.Name, p.Code, p.ImageUrl))
            .ToListAsync();
    }
}
