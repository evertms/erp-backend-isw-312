using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public static class InventorySeeder
{
    public static async Task SeedAsync(InventoryDbContext context, Guid defaultCompanyId)
    {
        // 1. Seed Categorías
        if (!await context.Categories.AnyAsync(c => c.CompanyId == defaultCompanyId))
        {
            var categories = new[]
            {
                Category.Create(defaultCompanyId, "Bebidas", "Gaseosas, jugos y bebidas alcohólicas"),
                Category.Create(defaultCompanyId, "Comidas", "Platos principales y entradas"),
                Category.Create(defaultCompanyId, "Postres", "Dulces y helados")
            };
            
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // 2. Seed Unidades
        if (!await context.Units.AnyAsync(u => u.CompanyId == defaultCompanyId))
        {
            var units = new[]
            {
                Unit.Create(defaultCompanyId, "Unidad", "UN"),
                Unit.Create(defaultCompanyId, "Litro", "LT"),
                Unit.Create(defaultCompanyId, "Kilogramo", "KG"),
                Unit.Create(defaultCompanyId, "Porción", "POR")
            };

            context.Units.AddRange(units);
            await context.SaveChangesAsync();
        }

        // 3. Seed Almacenes
        if (!await context.Warehouses.AnyAsync(w => w.CompanyId == defaultCompanyId))
        {
            var warehouse = Warehouse.Create(defaultCompanyId, "Almacén Principal", "Almacén central del restaurante");
            context.Warehouses.Add(warehouse);
            await context.SaveChangesAsync();
        }
    }
}
