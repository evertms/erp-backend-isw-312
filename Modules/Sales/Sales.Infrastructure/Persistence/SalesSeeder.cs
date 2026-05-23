using Microsoft.EntityFrameworkCore;
using Sales.Domain.Entities;
using Sales.Domain.Enums;

namespace Sales.Infrastructure.Persistence;

public static class SalesSeeder
{
    public static async Task SeedAsync(SalesDbContext context, string defaultCompanyCen)
    {
        // 1. Configuración de Impuestos
        var taxConfig = await context.TaxConfigurations.FirstOrDefaultAsync(t => t.CompanyCen == defaultCompanyCen);
        if (taxConfig == null)
        {
            taxConfig = TaxConfiguration.Create(defaultCompanyCen, 13.0m);
            context.TaxConfigurations.Add(taxConfig);
        }

        // 2. Configuración KDS
        if (!await context.StationCategoryConfigs.AnyAsync(s => s.CompanyCen == defaultCompanyCen))
        {
            var configCocina = StationCategoryConfig.Create(defaultCompanyCen, "CAT-ALIMENTOS", Station.Cocina);
            var configBar = StationCategoryConfig.Create(defaultCompanyCen, "CAT-BEBIDAS", Station.Bar);
            
            context.StationCategoryConfigs.Add(configCocina);
            context.StationCategoryConfigs.Add(configBar);
        }

        await context.SaveChangesAsync();
    }
}
