using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public static class InventorySeeder
{
    public static async Task SeedAsync(InventoryDbContext context, string defaultCompanyCen)
    {
        // 1. Seed Categorías
        if (!await context.Categories.AnyAsync(c => c.CompanyCen == defaultCompanyCen))
        {
            var categories = new[]
            {
                Category.Create(defaultCompanyCen, "Bebidas", "Gaseosas, jugos y bebidas alcohólicas"),
                Category.Create(defaultCompanyCen, "Comidas", "Platos principales y entradas"),
                Category.Create(defaultCompanyCen, "Postres", "Dulces y helados")
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        // 2. Seed Unidades
        if (!await context.Units.AnyAsync(u => u.CompanyCen == defaultCompanyCen))
        {
            var units = new[]
            {
                Unit.Create(defaultCompanyCen, "Unidad", "UN"),
                Unit.Create(defaultCompanyCen, "Litro", "LT"),
                Unit.Create(defaultCompanyCen, "Kilogramo", "KG"),
                Unit.Create(defaultCompanyCen, "Porción", "POR")
            };

            context.Units.AddRange(units);
            await context.SaveChangesAsync();
        }

        // 3. Seed Almacenes
        if (!await context.Warehouses.AnyAsync(w => w.CompanyCen == defaultCompanyCen))
        {
            var warehouse = Warehouse.Create(defaultCompanyCen, "Almacén Principal", "Almacén central del restaurante");
            context.Warehouses.Add(warehouse);
            await context.SaveChangesAsync();
        }
    }
}
