using Core.Domain.Entities;
using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence;

public static class CoreSeeder
{
    public const string DefaultCompanyCen = "COM-DEV-001";

    public static async Task SeedAsync(CoreDbContext context)
    {
        var companiesToSeed = new List<(string Cen, string Name)>
        {
            (DefaultCompanyCen, "Empresa de Desarrollo S.A."),
            ("COM-DEV-002", "Restaurante La Tech-ina")
        };

        foreach (var data in companiesToSeed)
        {
            var company = await context.Companies.FirstOrDefaultAsync(c => c.Cen == data.Cen);

            if (company is null)
            {
                company = Company.CreateWithCen(data.Cen, data.Name);
                context.Companies.Add(company);
                await context.SaveChangesAsync();
            }

            // Crear usuario admin por defecto si no hay usuarios en la empresa
            if (!await context.Users.AnyAsync(u => u.CompanyId == company.Id))
            {
                var admin = User.Create(company.Id, $"Admin {data.Name}", Role.SuperAdmin);
                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
