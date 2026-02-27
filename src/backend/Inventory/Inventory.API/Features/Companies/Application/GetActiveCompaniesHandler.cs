using Microsoft.EntityFrameworkCore;
using Inventory.API.Shared.Infrastructure.Persistence.Models;

namespace Inventory.API.Features.Companies.Application;

public class GetActiveCompaniesHandler
{
    private readonly AppDbContext _db;

    public GetActiveCompaniesHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CompanyDto>> HandleAsync()
    {
        return await _db.Companies
            .Where(c => c.IsActive)
            .Select(c => new CompanyDto(c.Id, c.Name, c.ImageUrl))
            .ToListAsync();
    }
}
