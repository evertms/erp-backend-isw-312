using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Companies.Application;

public class GetCompanyWarehousesHandler
{
    private readonly AppDbContext _db;

    public GetCompanyWarehousesHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<WarehouseDto>> HandleAsync(Guid companyId)
    {
        return await _db.Warehouses
            .Where(w => w.CompanyId == companyId && w.IsActive == true)
            .Select(w => new WarehouseDto(w.Id, w.Name, w.Location))
            .ToListAsync();
    }
}
