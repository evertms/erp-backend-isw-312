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

            await context.SaveChangesAsync(); // <-- Guardar para poder obtener los IDs

            // 4. Seed Products
            if (!await context.Products.AnyAsync(p => p.CompanyCen == companyCen))
            {
                var categoryId = context.Categories.First(c => c.CompanyCen == companyCen).Id;
                var unitId = context.Units.First(u => u.CompanyCen == companyCen).Id;

                var products = new[]
                {
                    Product.Create(companyCen, categoryId, unitId, "Coca Cola 1L", 10.50m, "CC-1L", null, "https://example.com/coca.jpg", 10),
                    Product.Create(companyCen, categoryId, unitId, "Hamburguesa Clásica", 25.00m, "HAM-CLAS", null, "https://example.com/burger.jpg", 5),
                    Product.Create(companyCen, categoryId, unitId, "Helado de Chocolate", 15.00m, "HEL-CHOCO", null, "https://example.com/icecream.jpg", 2)
                };

                context.Products.AddRange(products);
            }

            index++;
        }

        await context.SaveChangesAsync();
    }
}
