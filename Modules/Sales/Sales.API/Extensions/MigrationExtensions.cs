using Microsoft.EntityFrameworkCore;
using Sales.Infrastructure.Persistence;
using Core.Infrastructure.Persistence;

namespace Sales.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        // 1. Core Module (Shared Data)
        var coreContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();
        await coreContext.Database.MigrateAsync();
        await CoreSeeder.SeedAsync(coreContext);

        // 2. Sales Module
        var salesContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
        await salesContext.Database.MigrateAsync();
        await SalesSeeder.SeedAsync(salesContext);
    }
}
