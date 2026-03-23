using Core.Domain.Entities;
using Core.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.Repositories;

public class CompanyRepository(CoreDbContext dbContext) : ICompanyRepository
{
    public async Task<List<Company>> GetActiveCompaniesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Companies
            .Where(c => c.IsActive)
            .ToListAsync(cancellationToken);
    }
}
