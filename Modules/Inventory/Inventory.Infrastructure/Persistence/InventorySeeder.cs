using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence;

public static class InventorySeeder
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        var companies = new[] { "COM-DEV-001", "COM-DEV-002" };
        var index = 1;

        foreach (var companyCen in companies)
        {
            // 1. Seed Categorías
            if (!await context.Categories.AnyAsync(c => c.CompanyCen == companyCen))
            {
                var categories = new[]
                {
                    Category.Create(companyCen, "Bebidas", "Gaseosas, jugos y bebidas alcohólicas"),
                    Category.Create(companyCen, "Comidas", "Platos principales y entradas"),
                    Category.Create(companyCen, "Postres", "Dulces y helados")
                };

                context.Categories.AddRange(categories);
            }

            // 2. Seed Unidades
            if (!await context.Units.AnyAsync(u => u.CompanyCen == companyCen))
            {
                var units = new[]
                {
                    Unit.Create(companyCen, "Unidad", "UN"),
                    Unit.Create(companyCen, "Litro", "LT"),
                    Unit.Create(companyCen, "Kilogramo", "KG"),
                    Unit.Create(companyCen, "Porción", "POR")
                };

                context.Units.AddRange(units);
            }

            // 3. Seed Almacenes
            if (!await context.Warehouses.AnyAsync(w => w.CompanyCen == companyCen))
            {
                var warehouseCen = $"WH-DEMO-00{index}";
                var warehouse = Warehouse.CreateWithCen(warehouseCen, companyCen, $"Almacén Principal {index}", "Almacén central del restaurante");
                context.Warehouses.Add(warehouse);
            }

            index++;
        }

        await context.SaveChangesAsync();
    }
}
