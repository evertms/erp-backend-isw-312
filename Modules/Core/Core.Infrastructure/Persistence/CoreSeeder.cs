using Core.Domain.Entities;
using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence;

public static class CoreSeeder
{
    public const string DefaultCompanyCen = "COM-DEV-001";

    public static async Task SeedAsync(CoreDbContext context)
    {
        // Buscar si ya existe alguna empresa
        var company = await context.Companies.FirstOrDefaultAsync(c => c.Cen == DefaultCompanyCen);
        
        if (company is null)
        {
            company = Company.CreateWithCen(DefaultCompanyCen, "Empresa de Desarrollo S.A.");
            context.Companies.Add(company);
            await context.SaveChangesAsync();
        }

        // Crear usuario admin por defecto si no hay usuarios en la empresa
        if (!await context.Users.AnyAsync(u => u.CompanyId == company.Id))
        {
            var admin = User.Create(company.Id, "Admin Dev", Role.SuperAdmin);
            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
