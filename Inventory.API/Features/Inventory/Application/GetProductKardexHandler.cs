using Inventory.API.Shared.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Features.Inventory.Application;

public class GetProductKardexHandler
{
    private readonly AppDbContext _db;

    public GetProductKardexHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<KardexMovementDto>> HandleAsync(Guid productId)
    {
        var kardex = await _db.KardexMovements
            .Where(k => k.ProductId == productId)
            .OrderByDescending(k => k.MovementDate)
            .Select(k => new KardexMovementDto(
                k.MovementType,
                k.MovementDate,
                k.Quantity,
                k.Balance,
                k.Reason
            ))
            .ToListAsync();

        return kardex;
    }
}
