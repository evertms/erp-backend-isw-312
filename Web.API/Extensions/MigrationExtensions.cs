using Core.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Web.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAndSeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var coreDbContext = scope.ServiceProvider.GetRequiredService<CoreDbContext>();

        await coreDbContext.Database.MigrateAsync();

        await CoreSeeder.SeedAsync(coreDbContext);        
    }
}
