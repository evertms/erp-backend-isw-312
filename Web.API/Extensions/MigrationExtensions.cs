using Core.Infrastructure.Persistence;
using Inventory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Web.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var coreDbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        var inventoryDbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        // 1. Aplicar migraciones pendientes
        await coreDbContext.Database.MigrateAsync();
        await inventoryDbContext.Database.MigrateAsync();

        // 2. Ejecutar Seeders (el de Core devuelve el CompanyId generado)
        var companyId = await CoreSeeder.SeedAsync(coreDbContext);
        
        // Ejecutar Seeder de Inventory pasándole el CompanyId
        await InventorySeeder.SeedAsync(inventoryDbContext, companyId);
    }
}
