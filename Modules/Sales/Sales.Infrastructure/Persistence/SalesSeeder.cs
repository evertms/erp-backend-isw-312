using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Enums;

namespace Sales.Infrastructure.Persistence;

public static class SalesSeeder
{
    public static async Task SeedAsync(SalesDbContext context, string defaultCompanyCen)
    {
        var companies = new[] { defaultCompanyCen, "COM-DEV-002" };

        foreach (var companyCen in companies)
        {
            // 1. Configuración de Impuestos
            var taxConfig = await context.TaxConfigurations.FirstOrDefaultAsync(t => t.CompanyCen == companyCen);
            if (taxConfig == null)
            {
                taxConfig = TaxConfiguration.Create(companyCen, 13.0m);
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
        }

        await context.SaveChangesAsync();
    }
}
