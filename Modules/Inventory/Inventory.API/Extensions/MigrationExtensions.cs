using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        await context.Database.MigrateAsync();

        // Nota: Se requiere un CompanyId por defecto para el seeder de inventario.
        // En una implementación real esta Guid vendría de un tenant o una configuración.
        var defaultCompanyId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        await InventorySeeder.SeedAsync(context, defaultCompanyId);        
    }
}
