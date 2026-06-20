using Inventory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var coreContext = scope.ServiceProvider.GetRequiredService<Core.Infrastructure.Persistence.CoreDbContext>();
        await coreContext.Database.MigrateAsync();
        await Core.Infrastructure.Persistence.CoreSeeder.SeedAsync(coreContext);

        var context = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

        await context.Database.MigrateAsync();

        await InventorySeeder.SeedAsync(context);
    }
}
