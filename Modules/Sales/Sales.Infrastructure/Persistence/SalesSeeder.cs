using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Enums;

namespace Sales.Infrastructure.Persistence;

public static class SalesSeeder
{
    public static async Task SeedAsync(SalesDbContext context)
    {
        var companies = new[] { "COM-DEV-001", "COM-DEV-002" };
        var index = 1;

        foreach (var companyCen in companies)
        {
            // 1. Configuración de Impuestos
            var taxConfig = await context.TaxConfigurations.FirstOrDefaultAsync(t => t.CompanyCen == companyCen);
            if (taxConfig == null)
            {
                taxConfig = TaxConfiguration.Create(companyCen, 0.13m);
                context.TaxConfigurations.Add(taxConfig);
            }

            // 2. Configuración KDS
            if (!await context.StationCategoryConfigs.AnyAsync(s => s.CompanyCen == companyCen))
            {
                var configCocina = StationCategoryConfig.Create(companyCen, "CAT-ALIMENTOS", Station.Cocina);
                var configBar = StationCategoryConfig.Create(companyCen, "CAT-BEBIDAS", Station.Bar);

                context.StationCategoryConfigs.Add(configCocina);
                context.StationCategoryConfigs.Add(configBar);
            }

            // 3. Configuración de Ventas (Bodega por Defecto)
            if (!await context.SalesConfigurations.AnyAsync(s => s.CompanyCen == companyCen))
            {
                var salesConfig = SalesConfiguration.Create(companyCen, $"WH-DEMO-00{index}");
                context.SalesConfigurations.Add(salesConfig);
            }

            index++;
        }

        await context.SaveChangesAsync();
    }
}
