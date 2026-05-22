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

        // Use a known CEN or let the seeder handle it.
        // For development, we'll assume the company CEN "COM-DEV-001" exists or the seeder creates it.
        await InventorySeeder.SeedAsync(context, "COM-DEV-001");        
        }}
