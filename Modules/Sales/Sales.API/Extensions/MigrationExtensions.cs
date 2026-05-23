using Microsoft.EntityFrameworkCore;
using Sales.Infrastructure.Persistence;

namespace Sales.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var context = scope.ServiceProvider.GetRequiredService<SalesDbContext>();

        await context.Database.MigrateAsync();

        await SalesSeeder.SeedAsync(context, "COM-DEV-001");
    }
}
