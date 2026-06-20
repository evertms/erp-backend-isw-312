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

        await InventorySeeder.SeedAsync(context);
    }
}
